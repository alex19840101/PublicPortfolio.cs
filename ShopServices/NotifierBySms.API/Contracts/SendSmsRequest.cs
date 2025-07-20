namespace NotifierBySms.API.Contracts
{
    /// <summary> Запрос на отправку SMS-уведомления </summary>
    public class SendSmsRequest
    {
        /// <summary> *Телефон отправителя </summary>
        public string PhoneSender { get; internal set; } = default!;

        /// <summary> *Телефон получателя </summary>
        public string PhoneReceiver { get; internal set; } = default!;

        /// <summary> *Сообщение </summary>
        public string Message { get; internal set; } = default!;
    }
}
