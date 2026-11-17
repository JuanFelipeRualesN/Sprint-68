namespace TeachersAPI.Domain.Models.Lessons
{
    public class TemplateCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CssCode { get; set; }
        public string CssDynamicCode { get; set; }
        public string HtmlCode { get; set; }
        public string HtmlDynamicCode { get; set; }
        public string JavascriptCode { get; set; }
        public string JavascriptDynamicCode { get; set; }
        public string ObjectDynamicCode { get; set; }
        public string CssAnimationDynamic { get; set; }
        public string ImgTemplateRequirements { get; set; }
        public string RequireCharacterAnimationImg { get; set; }
        public string RequireCharacterAnimationFrames { get; set; }
        public string RequireCharacterAnimationAudio { get; set; }
        public string RequireObjectTouchImg { get; set; }
        public string RequireObjectTouchImgFrames { get; set; }
        public string RequireObjectTouchAudio { get; set; }
        public string RequireObjectDropImg { get; set; }
        public string RequireObjectDropImgFrames { get; set; }
        public string RequireObjectDropAudio { get; set; }
        public string RequireBackgroundImg { get; set; }
        public string RequireBackgroundImgInfiniteScroll { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}