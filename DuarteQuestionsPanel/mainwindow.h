#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QJsonArray>
#include <vector>

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
        CreateQuestion,
        DeleteAnswer,
        DeleteQuestion,
        RestoreAllAnswer,
        RestoreAllQuestion
    };

    enum UserDisplay
    {
        NoneUserDisplay,
        ReadUserDisplay,
        DeleteUserDisplay
    };

    enum QuestionDisplay
    {
        NoneQuestionDisplay,
        ReadQuestionDisplay,
        DeleteQuestionDisplay
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
    void on_buttonReloadTree_clicked();
    void on_buttonDeleteQuestion_clicked();
    void on_buttonRestoreAllQuestion_clicked();

private:
    void connectSlots();
    void populateTableWidget(QTableWidget *tableWidget, const QJsonArray &userArray);
    void onRequestError(QNetworkReply *reply);
    void onCreateUser(QNetworkReply *reply);
    void onGetUserList(QNetworkReply *reply);
    void onChangePassword(QNetworkReply *reply);
    void onDeleteUser(QNetworkReply *reply);
    void onRestoreUser(QNetworkReply *reply);
    void onGetQuestionList(QNetworkReply *reply);
    void onCreateAnswer(QNetworkReply *reply);
    void onCreateQuestion(QNetworkReply *reply);
    void onDeleteAnswer(QNetworkReply *reply);
    void onDeleteQuestion(QNetworkReply *reply);
    void onRestoreAllAnswer(QNetworkReply *reply);
    void onRestoreAllQuestion(QNetworkReply *reply);

    Ui::MainWindow *ui;
    QuestionDisplay m_questionDisplay;
    UserDisplay m_userDisplay;
    NetOperation m_currentOperation;
    QNetAccessManager *m_netManager;
    QJsonArray m_answerDtoArray;

    bool m_allAnswerRestored;
    bool m_allQuestionRestored;
    int m_deletedAnswerCount;
    int m_deletedQuestionCount;
    std::vector<int> m_answerList;
    std::vector<int> m_questionList;
};

#endif // MAINWINDOW_H
