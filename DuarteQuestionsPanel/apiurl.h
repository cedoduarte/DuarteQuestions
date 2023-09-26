#ifndef APIURL_H
#define APIURL_H

#include <QString>

class ApiUrl
{
public:
    static const QString ApiCreateUserUrl;
    static const QString ApiGetUserListUrl;
    static const QString ApiChangeUserPasswordUrl;
    static const QString ApiDeleteUserUrl;
    static const QString ApiRestoreUserUrl;
    static const QString ApiLoginUrl;
    static const QString ApiGetQuestionListUrl;
    static const QString ApiCreateAnswerUrl;
    static const QString ApiCreateQuestionUrl;
    static const QString ApiDeleteAnswerUrl;
    static const QString ApiDeleteQuestionUrl;
    static const QString ApiRestoreAllQuestionUrl;
    static const QString ApiRestoreAllAnswerUrl;

    ApiUrl() = delete;
};

#endif // APIURL_H
