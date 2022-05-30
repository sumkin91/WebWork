namespace WebWork.Infrastructure.Middleware;

public class TestMiddleware
{
    private readonly RequestDelegate _Next;
    
    public TestMiddleware(RequestDelegate Next) => _Next = Next;

    public async Task Invoke(HttpContext Context)
    {
        //обработка данных внутри Context
        //Запрос
        //Context.Request

        //Ответ
        //Context.Response

        //при модификации Body не должны вызывать Next
        //при вызове Next не должны модифицировать Body

        await _Next(Context); //без вызова прерывается работа конвеера

        //постобработка данных из Context.Response
    }
}
