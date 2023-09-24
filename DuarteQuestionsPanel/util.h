#ifndef UTIL_H
#define UTIL_H

#include <QString>

class Util
{
public:
    static const QString EmailRegexPattern;

    static bool isEmail(const QString &input);
    Util() = delete;
};

#endif // UTIL_H
