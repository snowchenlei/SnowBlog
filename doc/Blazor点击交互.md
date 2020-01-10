事件传递参数 https://docs.microsoft.com/zh-cn/aspnet/core/blazor/components?view=aspnetcore-3.1#lambda-expressions
```csharp
@for (int i = 1; i < 4; i++)
{
    // 不要直接在 lambda 表达式中使用 for 循环中的循环变量（i）。 否则，所有 lambda 表达式都将使用相同的变量，导致 i的值在所有 lambda 中都相同。 始终捕获其在本地变量中的值（在前面的示例中为buttonNumber），然后使用它。
    int page = i;
    <li class="page-item"><a class="page-link" href="/@i" @onclick="@(e=>ToPage(e, page))">@i</a></li>
}
@code{
    private void ToPage(MouseEventArgs e, int page)
    {
        // 执行逻辑
    }
}
```