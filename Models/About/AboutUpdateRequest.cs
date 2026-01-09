namespace PortfolioAdminApi.Models.About
{
    public class AboutUpdateRequest
    {
        public string IntroText { get; set; }
        public List<string> FrontendSkills { get; set; }
        public List<string> BackendSkills { get; set; }
        public List<string> DatabaseSkills { get; set; }
        public List<string> ToolsSkills { get; set; }
    }

}
