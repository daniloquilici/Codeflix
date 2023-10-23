namespace quilici.Codeflix.UnitTest.Application.Category.UpdateCategory
{
    public class UpdateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
        {
            var fixture = new UpdateCategoryTestFixture();
            for (int i = 0; i < times; i++)
            {
                var exampleCategore = fixture.GetExampleCategory();
                var exampleInput = fixture.GetValidInput(exampleCategore.Id);
                yield return new object[] { exampleCategore, exampleInput };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();
            int totalInvalidCases = 4;


            for (int i = 0; i < times; i++)
            {
                switch (i % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] { fixture.GetInvalidInputShortName(), "Name should be at least 3 characters long" });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] { fixture.GetInvalidInputLongName(), "Name should be less or equal 255 characters long" });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] { fixture.CreateCategoryInputTooLongDescription(), "Description should be less or equal 10000 characters long" });
                        break;
                    default:
                        break;
                }
            }

            return invalidInputsList;
        }
    }
}
