#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QJsonArray>

class QNetworkReply;
class QNetAccessManager;
class QTableWidget;

namespace Ui
{
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
public:
    enum NetOperation
    {
        NoneOperation,
        CreateUser,
        GetUserList,
        ChangePassword,
        DeleteUser,
        RestoreUser,
        GetQuestionList,
        CreateAnswer,
        CreateQuestion
    };

    enum TableWidgetDisplay
    {
        NoneTableWidgetDisplay,
        TableWidgetReadUsers,
        TableWidgetDeleteUsers
    };

    enum ReadUserColumn
    {
        IdReadUser,
        UsernameReadUser,
        EmailReadUser
    };

    explicit MainWindow(QWidget *parent = nullptr);
    virtual ~MainWindow();
private slots:
    void on_actionQuit_triggered();
    void on_buttonCreateUser_clicked();
    void requestFinished(QNetworkReply *reply);
    void on_buttonReloadRead_clicked();
    void on_buttonChangePassword_clicked();
    void on_buttonReloadDelete_clicked();
    void on_buttonDeleteUser_clicked();
    void on_buttonRestoreUser_clicked();
    void on_buttonReloadQuestions_clicked();
    void on_buttonAddAnswer_clicked();
    void on_buttonCreateQuestion_clicked();
    void on_buttonDeleteAnswer_clicked();
private:
    void connectSlots();
    void populateTableWidget(QTableWidget *tableWidget, const QJsonArray &userArray);

    Ui::MainWindow *ui;
    TableWidgetDisplay m_tableWidgetDisplay;
    NetOperation m_currentOperation;
    QNetAccessManager *m_netManager;
    QJsonArray m_answerDtoArray;
};

#endif // MAINWINDOW_H
