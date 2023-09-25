#include "mainwindow.h"
#include "./ui_mainwindow.h"
#include "qnetaccessmanager.h"
#include "qflexiblejsonobject.h"
#include "util.h"
#include "apiurl.h"
#include "questionlistcontainer.h"
#include "treeitemcheckbox.h"

#include <vector>
#include <QDebug>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonValue>
#include <QJsonParseError>
#include <QMessageBox>
#include <QRegularExpression>
#include <QListWidgetItem>
#include <QTreeWidgetItem>
#include <QRadioButton>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);    
    m_questionDisplay = NoneQuestionDisplay;
    m_userDisplay = NoneUserDisplay;
    m_currentOperation = NoneOperation;
    m_netManager = new QNetAccessManager(this);
    ui->tableWidgetDeleteUsers->setSelectionBehavior(QTableWidget::SelectRows);
    ui->tableWidgetDeleteUsers->setSelectionMode(QTableWidget::SingleSelection);
    ui->tabWidget->setCurrentIndex(0);
    ui->tabUserCRUD->setCurrentIndex(0);
    ui->tabQuestionCRUD->setCurrentIndex(0);
    ui->scrollArea->setWidget(nullptr);
    ui->treeWidgetQuestions->setColumnCount(1);
    ui->treeWidgetQuestions->setHeaderLabel("Questions");
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
    m_userDisplay = ReadUserDisplay;
    m_currentOperation = GetUserList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonReloadDelete_clicked()
{
    QString keyword = ui->txtReadUserKeyword->text();
    QString urlString = ApiUrl::ApiGetUserListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_userDisplay = DeleteUserDisplay;
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
    QString keyword = ""; // todo... poner filtro
    QString urlString = ApiUrl::ApiGetQuestionListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_questionDisplay = ReadQuestionDisplay;
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

void MainWindow::on_buttonReloadTree_clicked()
{
    QString keyword = ""; // todo... poner filtro
    QString urlString = ApiUrl::ApiGetQuestionListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_questionDisplay = DeleteQuestionDisplay;
    m_currentOperation = GetQuestionList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

void MainWindow::on_buttonDeleteQuestion_clicked()
{
    std::vector<int> answerList;
    std::vector<int> questionList;
    const int questionCount = ui->treeWidgetQuestions->topLevelItemCount();
    for (int xq = 0; xq < questionCount; ++xq)
    {
        QTreeWidgetItem *questionItem = ui->treeWidgetQuestions->topLevelItem(xq);
        TreeItemCheckBox *questionCheckBox = dynamic_cast<TreeItemCheckBox*>(
            ui->treeWidgetQuestions->itemWidget(questionItem, 0));
        if (questionCheckBox->isChecked())
        {
            questionList.push_back(questionCheckBox->data().toInt());
        }
        const int answerCount = ui->treeWidgetQuestions->topLevelItem(xq)->childCount();
        for (int xa = 0; xa < answerCount; ++xa)
        {
            QTreeWidgetItem *answerItem = ui->treeWidgetQuestions->topLevelItem(xq)->child(xa);
            TreeItemCheckBox *answerCheckBox = dynamic_cast<TreeItemCheckBox*>(
                ui->treeWidgetQuestions->itemWidget(answerItem, 0));
            if (answerCheckBox->isChecked())
            {
                answerList.push_back(answerCheckBox->data().toInt());
            }
        }
    }
    // delete all selected answers
    for (int answerId : answerList)
    {
        QString urlString = ApiUrl::ApiDeleteAnswerUrl + "/" + QString::number(answerId);
        m_currentOperation = DeleteAnswer;
        m_netManager->deleteResource(QNetRequest(QUrl(urlString)));

    }
    // delete all selected questions
    for (int questionId : questionList)
    {
        QString urlString = ApiUrl::ApiDeleteQuestionUrl + "/" + QString::number(questionId);
        m_currentOperation = DeleteQuestion;
        m_netManager->deleteResource(QNetRequest(QUrl(urlString)));
    }
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
                if (m_userDisplay == ReadUserDisplay)
                {
                    populateTableWidget(ui->tableWidgetReadUsers, QJsonDocument::fromJson(reply->readAll()).array());
                }
                else if (m_userDisplay == DeleteUserDisplay)
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
                if (m_questionDisplay == ReadQuestionDisplay)
                {
                    delete ui->scrollArea->widget();
                    ui->scrollArea->setWidget(new QuestionListContainer(
                        QJsonDocument::fromJson(reply->readAll()).array(), this));
                }
                else if (m_questionDisplay == DeleteQuestionDisplay)
                {
                    // clear question tree
                    while (ui->treeWidgetQuestions->topLevelItemCount() != 0)
                    {
                        delete ui->treeWidgetQuestions->takeTopLevelItem(0);
                    }
                    QJsonArray questionArray = QJsonDocument::fromJson(reply->readAll()).array();
                    for (const QJsonValue &questionValue : questionArray)
                    {
                        QTreeWidgetItem *questionItem = new QTreeWidgetItem;
                        TreeItemCheckBox *questionCheckBox = new TreeItemCheckBox(this);
                        questionCheckBox->setText(QFlexibleJsonObject::value(questionValue.toObject(), "text").toString());
                        questionCheckBox->setData(QFlexibleJsonObject::value(questionValue.toObject(), "id").toInt());
                        QJsonArray answerArray = QFlexibleJsonObject::value(questionValue.toObject(), "answers").toArray();
                        for (const QJsonValue &answerValue : answerArray)
                        {
                            if (!QFlexibleJsonObject::value(answerValue.toObject(), "isDeleted").toBool())
                            {
                                QTreeWidgetItem *answerItem = new QTreeWidgetItem;
                                answerItem->setText(0, QFlexibleJsonObject::value(answerValue.toObject(), "text").toString());
                                answerItem->setData(0, Qt::UserRole, QFlexibleJsonObject::value(answerValue.toObject(), "id").toInt());
                                questionItem->addChild(answerItem);
                            }
                        }
                        ui->treeWidgetQuestions->addTopLevelItem(questionItem);
                        ui->treeWidgetQuestions->setItemWidget(questionItem, 0, questionCheckBox);
                        for (int i = 0; i < questionItem->childCount(); ++i)
                        {
                            TreeItemCheckBox *answerCheckBox = new TreeItemCheckBox(this);
                            answerCheckBox->setText(questionItem->child(i)->text(0));
                            answerCheckBox->setData(questionItem->child(i)->data(0, Qt::UserRole).toInt());
                            ui->treeWidgetQuestions->setItemWidget(questionItem->child(i), 0, answerCheckBox);
                            questionItem->child(i)->setText(0, nullptr);
                        }
                    }
                }
                break;
            }
            case CreateAnswer:
            {
                m_answerDtoArray.append(QJsonDocument::fromJson(reply->readAll()).object());
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
                        for (const QJsonValue &dto : qAsConst(m_answerDtoArray))
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
            case DeleteAnswer:
            {
                // todo... mostrar las respuestas que fueron eliminadas
                qDebug() << "answer deleted:" << QString::fromLatin1(reply->readAll());
                break;
            }
            case DeleteQuestion:
            {
                // todo... mostrar las preguntas que fueron eliminadas, junto a sus respuestas
                qDebug() << "question deleted:" << QString::fromLatin1(reply->readAll());
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
