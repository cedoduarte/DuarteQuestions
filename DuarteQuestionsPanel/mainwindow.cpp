#include "mainwindow.h"
#include "./ui_mainwindow.h"
#include "qnetaccessmanager.h"
#include "qflexiblejsonobject.h"
#include "util.h"
#include "apiurl.h"

#include <QDebug>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonValue>
#include <QJsonParseError>
#include <QMessageBox>
#include <QRegularExpression>
#include <QRadioButton>
#include <QVBoxLayout>
#include <QHBoxLayout>

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

QFrame *MainWindow::makeQuestionFrame(const QString &questionText, const std::vector<QString> &answerList)
{
    QFrame *questionFrame = new QFrame(this);
    questionFrame->setFrameShape(QFrame::Box);
    QVBoxLayout *frameLayout = new QVBoxLayout;
    questionFrame->setLayout(frameLayout);
    QLabel *questionLabel = new QLabel(questionText, questionFrame);
    frameLayout->addWidget(questionLabel);
    for (const QString &answer : answerList)
    {
        frameLayout->addWidget(new QRadioButton(answer, this));
    }
    return questionFrame;
}

void MainWindow::connectSlots()
{
    connect(m_netManager, &QNetAccessManager::finished, this, &MainWindow::requestFinished);
}

void MainWindow::on_actionQuit_triggered()
{
    close();
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
                QWidget *containerWidget = new QWidget(ui->scrollArea);
                QVBoxLayout *containerLayout = new QVBoxLayout;
                containerWidget->setLayout(containerLayout);
                QJsonArray questionArray = QJsonDocument::fromJson(reply->readAll()).array();
                for (const QJsonValue &questionValue : questionArray)
                {
                    QString questionText = QFlexibleJsonObject::value(questionValue.toObject(), "text").toString();
                    QJsonArray answerArray = QFlexibleJsonObject::value(questionValue.toObject(), "answers").toArray();
                    std::vector<QString> answerList;
                    for (const QJsonValue &answerValue : answerArray)
                    {
                        QString answerText = QFlexibleJsonObject::value(answerValue.toObject(), "text").toString();
                        answerList.push_back(answerText);
                    }
                    containerLayout->addWidget(makeQuestionFrame(questionText, answerList));
                }
                ui->scrollArea->setWidget(containerWidget);
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
        systemExceptionString = systemExceptionString.mid(0, systemExceptionString.indexOf("!")).simplified();
        QMessageBox::warning(this, "Warn", systemExceptionString);
    }
}

void MainWindow::populateTableWidget(QTableWidget *tableWidget, const QJsonArray &userArray)
{
    tableWidget->setRowCount(0);
    for (const QJsonValue &value : userArray)
    {
        int id = QFlexibleJsonObject::value(value.toObject(), "id").toInt();
        QString name = QFlexibleJsonObject::value(value.toObject(), "name").toString();
        QString email = QFlexibleJsonObject::value(value.toObject(), "email").toString();
        const int row = tableWidget->rowCount();
        tableWidget->insertRow(row);
        tableWidget->setItem(row, IdReadUser, new QTableWidgetItem(QString::number(id)));
        tableWidget->setItem(row, UsernameReadUser, new QTableWidgetItem(name));
        tableWidget->setItem(row, EmailReadUser, new QTableWidgetItem(email));
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

void MainWindow::on_buttonReloadRead_clicked()
{
    QString keyword = ui->txtReadUserKeyword->text();
    QString urlString = ApiUrl::ApiGetUserListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_tableWidgetDisplay = TableWidgetReadUsers;
    m_currentOperation = GetUserList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
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

void MainWindow::on_buttonReloadQuestions_clicked()
{
    QString keyword = "";
    QString urlString = ApiUrl::ApiGetQuestionListUrl + (keyword.isEmpty() ? "?GetAll=true" : "?Keyword=" + keyword);
    m_currentOperation = GetQuestionList;
    m_netManager->get(QNetRequest(QUrl(urlString)));
}

