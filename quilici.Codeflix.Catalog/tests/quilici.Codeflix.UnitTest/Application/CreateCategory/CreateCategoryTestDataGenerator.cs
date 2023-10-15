namespace quilici.Codeflix.UnitTest.Application.CreateCategory
{
    public class CreateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreateCategoryTestFixture();
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
                        invalidInputsList.Add(new object[] { fixture.CreateCategoryInputInputDescriptionNull(), "Description should not be null" });
                        break;
                    case 3:
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
