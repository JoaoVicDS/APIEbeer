namespace APIEbeer.Services.Questions
{
    public interface IQuestionService
    {
        string GetQuestionForCharacteristic(string characteristicName);
    }
}