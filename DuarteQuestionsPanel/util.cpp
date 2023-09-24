#include "util.h"

#include <QRegularExpression>
#include <QRegularExpressionMatch>

const QString Util::EmailRegexPattern = "[a-z0-9._]+@[a-z]+\\.[a-z]{2,3}";

bool Util::isEmail(const QString &input)
{
    return QRegularExpression(EmailRegexPattern).match(input).hasMatch();
}
