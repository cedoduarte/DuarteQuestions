#ifndef LOGINDIALOG_H
#define LOGINDIALOG_H

#include <QDialog>

class QNetworkReply;
class QNetAccessManager;

namespace Ui
{
class LogInDialog;
}

class LogInDialog : public QDialog
{
    Q_OBJECT
public:
    explicit LogInDialog(QWidget *parent = nullptr);
    virtual ~LogInDialog();
private slots:
    void on_buttonEnter_clicked();
    void requestFinished(QNetworkReply *reply);
private:
    Ui::LogInDialog *ui;
    QNetAccessManager *m_netManager;
};

#endif // LOGINDIALOG_H
