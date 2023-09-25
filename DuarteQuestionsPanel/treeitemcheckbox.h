#ifndef TREEITEMCHECKBOX_H
#define TREEITEMCHECKBOX_H

#include <QCheckBox>
#include <QVariant>

class TreeItemCheckBox : public QCheckBox
{
    Q_OBJECT
public:
    explicit TreeItemCheckBox(QWidget *parent = nullptr);
    virtual ~TreeItemCheckBox();
    void setData(const QVariant &data);
    QVariant data() const;
private:
    QVariant m_data;
};

#endif // TREEITEMCHECKBOX_H
