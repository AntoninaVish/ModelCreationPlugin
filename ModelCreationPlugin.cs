using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCreationPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class ModelCreationPlugin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document; //получаем доступ к документу


            //1 вариант
            var res1 = new FilteredElementCollector(doc) //находим все  стены в проекте
                .OfClass(typeof(WallType))//OfClass относится к категории быстрых фильтров, этофильтры Revit
                //.Cast<Wall>()//метод, который выполняет преобразование каждого элемента в списке к заданному типу
                .OfType<WallType>()//этот метод выпоняет фильтрацию на основе заданного типа, он не выполняет преобразование в стену,
                               //а отбирает в списке все стены, используется для безопасного приведения списка из типа ListElement в тип ListWall
                .ToList(); // на выходе получаем список стен

            //2 вариант
            //пример с загружаемыми семействами, загружаемые семейства определяются типами FamilySymbol и FamilyInstance
            //будем выполнять фильтрацию по типу FamilyInstance

            var res2 = new FilteredElementCollector(doc) //находим все  стены в проекте
                    .OfClass(typeof(FamilyInstance))//OfClass относится к категории быстрых фильтров, этофильтры Revit
                    .OfCategory(BuiltInCategory.OST_Doors) //для загружаемых семейств при поиске именно по категории позволяет отделить одни семейства от других
                     //.Cast<Wall>()//метод, который выполняет преобразование каждого элемента в списке к заданному типу
                    .OfType<FamilyInstance>()//этот метод выпоняет фильтрацию на основе заданного типа, он не выполняет преобразование в стену,
                                             //а отбирает в списке все стены, используется для безопасного приведения списка из типа ListElement в тип ListWall
                     .Where(x=>x.Name.Equals("0915 x 2134 мм")) //свойство Name ищиться у типа FamilyInstance, потому что мы уже отфильтровали FamilyInstance по типу
                    .ToList(); // на выходе получаем список стен

            //3 вариант
            var res3 = new FilteredElementCollector(doc) //находим все  стены в проекте
                   .WhereElementIsNotElementType() // быстрый фильтр, этот фильтр отбирает те объекты, которые относяться к типоразмерам хоть системных хоть загружаемых семейств
                                             // фильтр WhereElementIsNotElementType - находит именно экземпляры т.е конкретно: стены, двери
                   .ToList(); // на выходе получаем список стен


            return Result.Succeeded;
        }
    }
}
