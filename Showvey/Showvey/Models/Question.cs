using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Question:Entity
    {
        public string Content { get; set; }
        public int Number { get; set; }
        public Guid QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public TimeSpan TimeLength { get; set; }
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public ICollection<Response> Responses { get; set; }
        public ICollection<Animate> Animates { get; set; }
        public bool IsFreeText { get; set; }
        public int Count { get; set; }
        public bool IsScale { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
    }
}