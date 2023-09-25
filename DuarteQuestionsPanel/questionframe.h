#ifndef QUESTIONFRAME_H
#define QUESTIONFRAME_H

#include <QFrame>
#include <vector>

class QLabel;
class QVBoxLayout;

class QuestionFrame : public QFrame
{
    Q_OBJECT
public:
    explicit QuestionFrame(const QString &questionText,
                            const std::vector<QString> &answerList,
                            QWidget *parent = nullptr);

    virtual ~QuestionFrame();
private:
    QVBoxLayout *m_vbox;
    QLabel *m_questionLabel;
};

#endif // QUESTIONFRAME_H
