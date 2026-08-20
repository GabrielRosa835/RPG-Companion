namespace RpgCompanion.Canva.Models;

using Core;

public static class ModelTest
{
    public static void Run(IDatabase database)
    {
        var model1 = new Model1.Model();
        var model2 = new Model2();

        model1.Set("Test", new ModelContent.Number(model1.NumberValue));



    }
}
