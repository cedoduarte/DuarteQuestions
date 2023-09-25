#include "questionlistcontainer.h"
#include "qflexiblejsonobject.h"
#include "questionframe.h"

#include <QVBoxLayout>

QuestionListContainer::QuestionListContainer(const QJsonArray &questionArray, QWidget *parent)
    : QWidget(parent)
{
    m_vbox = new QVBoxLayout;
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
        m_vbox->addWidget(new QuestionFrame(questionText, answerList));
    }
    setLayout(m_vbox);
}

QuestionListContainer::~QuestionListContainer()
{
}
