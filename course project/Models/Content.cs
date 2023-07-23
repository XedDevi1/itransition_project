namespace course_project.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string NameOfContent { get; set; }
        public string Description { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Collection Collection { get; set; }
    }
}
