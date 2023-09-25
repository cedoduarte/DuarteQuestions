#include "treeitemcheckbox.h"

TreeItemCheckBox::TreeItemCheckBox(QWidget *parent)
    : QCheckBox(parent)
{
}

TreeItemCheckBox::~TreeItemCheckBox()
{
}

void TreeItemCheckBox::setData(const QVariant &data)
{
    m_data = data;
}

QVariant TreeItemCheckBox::data() const
{
    return m_data;
}
