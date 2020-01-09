using System;

namespace Snow.Blog.Service.Dto
{
    public class EntityDto : EntityDto<int> { }

    public class EntityDto<TPrimaryKey>
    {
        public int ID { get; set; }
        public DateTime AddTime { get; set; }
    }
}