namespace NotifierByEmail.API.Contracts
{
    /// <summary> Запрос на отправку E-mail-письма </summary>
    public class SendEmailRequest
    {
        /// <summary> *E-mail-адрес отправителя </summary>
        public string EmailSender { get; internal set; } = default!;
        
        /// <summary> *E-mail-адрес получателя </summary>
        public string EmailReceiver { get; internal set; } = default!;

        /// <summary> *Тема сообщения (письма) </summary>
        public string Topic { get; internal set; } = default!;
        
        /// <summary> *Тело (Body) письма </summary>
        public string EmailBody { get; internal set; } = default!;
    }
}
