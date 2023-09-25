#include "questionframe.h"

#include <QVBoxLayout>
#include <QRadioButton>
#include <QLabel>

QuestionFrame::QuestionFrame(const QString &questionText, const std::vector<QString> &answerList, QWidget *parent)
    : QFrame(parent)
{
    m_questionLabel = new QLabel(questionText, this);
    m_vbox = new QVBoxLayout;
    m_vbox->addWidget(m_questionLabel);
    for (const QString &answer : answerList)
    {
        m_vbox->addWidget(new QRadioButton(answer, this));
    }
    setLayout(m_vbox);
}

QuestionFrame::~QuestionFrame()
{
}
