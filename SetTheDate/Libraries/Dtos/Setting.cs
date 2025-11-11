using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
