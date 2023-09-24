#include "logindialog.h"
#include "ui_logindialog.h"
#include "apiurl.h"
#include "qnetaccessmanager.h"

#include <QMessageBox>
#include <QJsonDocument>
#include <QJsonArray>
#include <QJsonObject>
#include <QJsonValue>
#include <QJsonParseError>

LogInDialog::LogInDialog(QWidget *parent)
    : QDialog(parent)
    , ui(new Ui::LogInDialog)
{
    ui->setupUi(this);
    m_netManager = new QNetAccessManager(this);
    connect(m_netManager, &QNetAccessManager::finished, this, &LogInDialog::requestFinished);
}

LogInDialog::~LogInDialog()
{
    delete ui;
}

void LogInDialog::requestFinished(QNetworkReply *reply)
{
    if (reply->error() == QNetworkReply::NoError)
    {
        QString result = QString::fromLatin1(reply->readAll());
        if (result == "true")
        {
            QMessageBox::information(this, "Ok", "Access granted!");
        }
        else if (result == "false")
        {
            QMessageBox::warning(this, "Warn", "Access denied!");
        }
    }
    else
    {
        QString systemExceptionString = QString::fromLatin1(reply->readAll());
        systemExceptionString = systemExceptionString.mid(0, systemExceptionString.indexOf("!")).simplified();
        QMessageBox::warning(this, "Warn", systemExceptionString);
    }
}

void LogInDialog::on_buttonEnter_clicked()
{
    QString usernameInput = ui->txtUsername->text();
    QString passwordInput = ui->txtPassword->text();
    if (usernameInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The username cannot be empty!");
        return;
    }
    if (passwordInput.isEmpty())
    {
        QMessageBox::warning(this, "Warn", "The password cannot be empty!");
        return;
    }
    QJsonDocument doc;
    doc.setObject({
        { "username", usernameInput },
        { "password", passwordInput },
    });
    m_netManager->post(QNetRequest(QUrl(ApiUrl::ApiLoginUrl)), doc.toJson(QJsonDocument::Compact));
}
