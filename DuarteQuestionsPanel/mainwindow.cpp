#include "mainwindow.h"
#include "./ui_mainwindow.h"
#include "qnetaccessmanager.h"
#include "qflexiblejsonobject.h"
#include "util.h"
#include "apiurl.h"
#include "questionlistcontainer.h"

#include <QDebug>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonValue>
#include <QJsonParseError>
#include <QMessageBox>
#include <QRegularExpression>
#include <QListWidgetItem>
#include <QRadioButton>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    m_tableWidgetDisplay = NoneTableWidgetDisplay;
    m_currentOperation = NoneOperation;
    m_netManager = new QNetAccessManager(this);
    ui->tableWidgetDeleteUsers->setSelectionBehavior(QTableWidget::SelectRows);
    ui->tableWidgetDeleteUsers->setSelectionMode(QTableWidget::SingleSelection);
    ui->tabWidget->setCurrentIndex(0);
    ui->tabUserCRUD->setCurrentIndex(0);
    ui->tabQuestionCRUD->setCurrentIndex(0);
    ui->scrollArea->setWidget(nullptr);
    connectSlots();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::connectSlots()
{
    connect(m_netManager, &QNetAccessManager::finished, this, &MainWindow::requestFinished);
}

void MainWindow::on_actionQuit_triggered()
{
    close();
}

void MainWindow::populateTableWidget(QTableWidget *tableWidget, const QJsonArray &userArray)
{
    tableWidget->setRowCount(0);
    for (const QJsonValue &value : userArray)
    {
        const int row = tableWidget->rowCount();
        tableWidget->insertRow(row);
        tableWidget->setItem(row, IdReadUser, new QTableWidgetItem(QString::number(QFlexibleJsonObject::value(
            value.toObject(), "id").toInt())));
        tableWidget->setItem(row, UsernameReadUser, new QTableWidgetItem(QFlexibleJsonObject::value(
            value.toObject(), "name").toString()));
        tableWidget->setItem(row, EmailReadUser, new QTableWidgetItem(QFlexibleJsonObject::value(
            value.toObject(), "email").toString()));
    }
}

void MainWindow::on_buttonCreateUser_clicked()
{
    QString usernameInput = ui->txtUsername->text();
    if (usernameInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The username cannot be empty!");
        return;
    }
    QString emailInput = ui->txtEmail->text();
    if (emailInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The email cannot be empty!");
        return;
    }
    if (!Util::isEmail(emailInput))
    {
        QMessageBox::warning(this, "Warn", "The email is not valid!");
        return;
    }
    QString confirmedEmailInput = ui->txtConfirmedEmail->text();
    if (emailInput != confirmedEmailInput)
    {
        QMessageBox::warning(this, "Warn", "The email needs to be confirmed!");
        return;
    }
    QString passwordInput = ui->txtPassword->text();
    if (passwordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The password cannot be empty!");
        return;
    }
    QString confirmedPasswordInput = ui->txtConfirmedPassword->text();
    if (passwordInput != confirmedPasswordInput)
    {
        QMessageBox::warning(this, "Warn", "The password needs to be confirmed!");
        return;
    }
    QJsonDocument doc;
    doc.setObject({
        { "name", usernameInput },
        { "email", emailInput },
        { "confirmedEmail", confirmedEmailInput },
        { "password", passwordInput },
        { "confirmedPassword", confirmedPasswordInput }
    });
    m_currentOperation = CreateUser;
    m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiCreateUserUrl)), doc.toJson(QJsonDocument::Compact));
}

void MainWindow::on_buttonChangePassword_clicked()
{
    QString nameEmailInput = ui->txtNameEmail->text();
    if (nameEmailInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The username cannot be empty!");
        return;
    }
    QString currentPasswordInput = ui->txtCurrentPassword->text();
    if (currentPasswordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The current password cannot be empty!");
        return;
    }
    QString newPasswordInput = ui->txtNewPassword->text();
    if (newPasswordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The new password cannot be empty!");
        return;
    }
    QString confirmedPasswordInput = ui->txtConfirmedNewPassword->text();
    if (confirmedPasswordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The confirmed password cannot be empty!");
        return;
    }
    if (newPasswordInput != confirmedPasswordInput)
    {
        QMessageBox::warning(this, "Warn", "The password is not confirmed!");
        return;
    }
    QJsonDocument doc;
    doc.setObject({
        { "username", nameEmailInput },
        { "currentPassword", currentPasswordInput },
        { "newPassword", newPasswordInput },
        { "confirmedPassword", confirmedPasswordInput }
    });
    m_currentOperation = ChangePassword;
    m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiChangeUserPasswordUrl)), doc.toJson(QJsonDocument::Compact));
}

void MainWindow::on_buttonRestoreUser_clicked()
{
    QString usernameInput = ui->txtUsernameRestore->text();
    if (usernameInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The username cannot be empty!");
        return;
    }
    QString passwordInput = ui->txtPasswordRestore->text();
    if (passwordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The password cannot be empty!");
        return;
    }
    QString confirmedPasswordInput = ui->txtConfirmedPasswordRestore->text();
    if (confirmedPasswordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The confirmed password cannot be empty!");
        return;
    }
    if (passwordInput != confirmedPasswordInput)
    {
        QMessageBox::warning(this, "Warn", "The password is not confirmed!");
        return;
    }
    QJsonDocument doc;
    doc.setObject({
        { "username", usernameInput },
        { "password", passwordInput },
        { "confirmedPassword", confirmedPasswordInput }
    });
    m_currentOperation = RestoreUser;
    m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiRestoreUserUrl)), doc.toJson(QJsonDocument::Compact));
}

void MainWindow::on_buttonCreateQuestion_clicked()
{
    QString questionInput = ui->txtQuestion->text();
    if (questionInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The question cannot be empty!");
        return;
    }
    if (ui->listWidgetAnswer->count() == 0)
    {
        QMessageBox::warning(this, "Warn", "The question needs at least one answer!");
        return;
    }
    // first of all we need to create the "answers" and must know their "IDs"
    for (int row = 0; row < ui->listWidgetAnswer->count(); ++row)
    {
        QRadioButton *answerRadio = dynamic_cast<QRadioButton*>(
            ui->listWidgetAnswer->itemWidget(ui->listWidgetAnswer->item(row)));
        QJsonDocument doc;
        doc.setObject({
            { "text", answerRadio->text() }
        });
        m_currentOperation = CreateAnswer;
        m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiCreateAnswerUrl)), doc.toJson(QJsonDocument::Compact));
    }
}

void MainWindow::on_buttonReloadRead_clicked()
{
    QString keyword = ui->txtReadUserKeyword->text();
    QString urlString = ApiUrl::ApiGetUserListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_tableWidgetDisplay = TableWidgetReadUsers;
    m_currentOperation = GetUserList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonReloadDelete_clicked()
{
    QString keyword = ui->txtReadUserKeyword->text();
    QString urlString = ApiUrl::ApiGetUserListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_tableWidgetDisplay = TableWidgetDeleteUsers;
    m_currentOperation = GetUserList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonDeleteUser_clicked()
{
    const int currentRow = ui->tableWidgetDeleteUsers->currentRow();
    const int selectedUserId = ui->tableWidgetDeleteUsers->item(currentRow, IdReadUser)->text().toInt();
    QString urlString = ApiUrl::ApiDeleteUserUrl + "/" + QString::number(selectedUserId);
    m_currentOperation = DeleteUser;
    m_netManager->deleteResource(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonReloadQuestions_clicked()
{
    QString keyword = "";
    QString urlString = ApiUrl::ApiGetQuestionListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_currentOperation = GetQuestionList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonAddAnswer_clicked()
{
    QString answerInput = ui->txtAnswer->text();
    if (answerInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The answer cannot be empty!");
        return;
    }
    QListWidgetItem *item = new QListWidgetItem;
    ui->listWidgetAnswer->addItem(item);
    ui->listWidgetAnswer->setItemWidget(item, new QRadioButton(answerInput, this));
}

void MainWindow::on_buttonDeleteAnswer_clicked()
{
    delete ui->listWidgetAnswer->takeItem(ui->listWidgetAnswer->currentRow());
}

void MainWindow::requestFinished(QNetworkReply *reply)
{
    if (reply->error() == QNetworkReply::NoError)
    {
        switch (m_currentOperation)
        {
            case CreateUser:
            {
                QString result = QString::fromLatin1(reply->readAll());
                if (result == "true")
                {
                    QMessageBox::information(this, "Ok", "User created successfully!");
                }
                break;
            }
            case ChangePassword:
            {
                QString result = QString::fromLatin1(reply->readAll());
                if (result == "true")
                {
                    QMessageBox::information(this, "Ok", "User's password was changed successfully!");
                }
                break;
            }
            case GetUserList:
            {
                if (m_tableWidgetDisplay == TableWidgetReadUsers)
                {
                    populateTableWidget(ui->tableWidgetReadUsers, QJsonDocument::fromJson(reply->readAll()).array());
                }
                else if (m_tableWidgetDisplay == TableWidgetDeleteUsers)
                {
                    populateTableWidget(ui->tableWidgetDeleteUsers, QJsonDocument::fromJson(reply->readAll()).array());
                }
                break;
            }
            case DeleteUser:
            {
                QString result = QString::fromLatin1(reply->readAll());
                if (result == "true")
                {
                    QMessageBox::information(this, "Ok", "User deleted successfully!");
                }
                break;
            }
            case RestoreUser:
            {
                QString result = QString::fromLatin1(reply->readAll());
                if (result == "true")
                {
                    QMessageBox::information(this, "Ok", "User restored successfully!");
                }
                break;
            }
            case GetQuestionList:
            {
                delete ui->scrollArea->widget();
                ui->scrollArea->setWidget(new QuestionListContainer(
                    QJsonDocument::fromJson(reply->readAll()).array(), this));
                break;
            }
            case CreateAnswer:
            {
                QJsonObject createAnswerDTO = QJsonDocument::fromJson(reply->readAll()).object();
                if (QFlexibleJsonObject::value(createAnswerDTO, "result").toBool())
                {
                    m_answerDtoArray.append(createAnswerDTO);
                    // the answer was created successfully with ID
                    if (m_answerDtoArray.size() == ui->listWidgetAnswer->count())
                    {
                        // all the answers were created successfully, now it is moment to create the question
                        QJsonArray answerIdList;
                        for (const QJsonValue &dto : qAsConst(m_answerDtoArray))
                        {
                            answerIdList.append(QFlexibleJsonObject::value(dto.toObject(), "id").toInt());
                        }

                        QString rightAnswerText;
                        for (int row = 0; row < ui->listWidgetAnswer->count(); ++row)
                        {
                            QRadioButton *answerRadio = dynamic_cast<QRadioButton*>(
                                ui->listWidgetAnswer->itemWidget(ui->listWidgetAnswer->item(row)));
                            if (answerRadio->isChecked())
                            {
                                rightAnswerText = answerRadio->text();
                                break;
                            }
                        }
                        int rightAnswerId = QFlexibleJsonObject::value(m_answerDtoArray.at(0).toObject(), "id").toInt();
                        if (!rightAnswerText.isEmpty())
                        {
                            for (const QJsonValue &dto :qAsConst(m_answerDtoArray))
                            {
                                QString answerText = QFlexibleJsonObject::value(dto.toObject(), "text").toString();
                                if (rightAnswerText == answerText)
                                {
                                    rightAnswerId = QFlexibleJsonObject::value(dto.toObject(), "id").toInt();
                                    break;
                                }
                            }
                        }

                        QJsonDocument doc;
                        doc.setObject({
                            { "text", ui->txtQuestion->text() },
                            { "answers", answerIdList },
                            { "rightAnswer", rightAnswerId }
                        });
                        m_currentOperation = CreateQuestion;
                        m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiCreateQuestionUrl)),
                                           doc.toJson(QJsonDocument::Compact));
                        // clear list of answers' DTOs
                        while (!m_answerDtoArray.empty())
                        {
                            m_answerDtoArray.pop_back();
                        }
                    }
                }
                break;
            }
            case CreateQuestion:
            {
                QString result = QString::fromLatin1(reply->readAll());
                if (result == "true")
                {
                    QMessageBox::information(this, "Ok", "Question created successfully!");
                }
                break;
            }
            case NoneOperation:
            {
                break;
            }
        }
    }
    else
    {
        QString systemExceptionString = QString::fromLatin1(reply->readAll());
        systemExceptionString = systemExceptionString.isEmpty() ? "Unknown error!"
                              : systemExceptionString.mid(0, systemExceptionString.indexOf("!")).simplified();
        QMessageBox::warning(this, "Warn", systemExceptionString);
    }
}
