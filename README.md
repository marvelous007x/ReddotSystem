## 简述

有针对红点类型id和针对具体GameObject手动处理两种方式。

红点更新使用脏数据标记然后在LateUpdate中统一处理，避免多次重复调用以及无注册时不必要的红点判断。

单纯红点，不带数值。具体红点显示自行在void Reddot.ShowReddot(bool active)中实现。

绑定红点类型id使用AutoReddot组件，每个GameObject单独自行处理使用ManualReddot组件。

ReddotEvent 用于 Manual Reddot，ReddotType 用于 AutoReddot。
