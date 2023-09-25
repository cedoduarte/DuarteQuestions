#ifndef QUESTIONLISTCONTAINER_H
#define QUESTIONLISTCONTAINER_H

#include <QWidget>
#include <QJsonArray>

class QVBoxLayout;

class QuestionListContainer : public QWidget
{
    Q_OBJECT
public:
    explicit QuestionListContainer(const QJsonArray &questionArray, QWidget *parent = nullptr);
    virtual ~QuestionListContainer();
private:
    QVBoxLayout *m_vbox;
};

#endif // QUESTIONLISTCONTAINER_H
