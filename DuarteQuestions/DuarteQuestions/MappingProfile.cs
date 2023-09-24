using AutoMapper;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.CQRS.Questions.ViewModel;
using DuarteQuestions.CQRS.Users.ViewModel;
using DuarteQuestions.Model;

namespace DuarteQuestions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Answer and AnswerViewModel
            CreateMap<Answer, AnswerViewModel>()
                .ForMember(a => a.Id, v => v.MapFrom(m => m.Id))
                .ForMember(a => a.Text, v => v.MapFrom(m => m.Text))
                .ForMember(a => a.IsDeleted, v => v.MapFrom(m => m.IsDeleted));
            CreateMap<AnswerViewModel, Answer>()
                .ForMember(v => v.Id, a => a.MapFrom(e => e.Id))
                .ForMember(v => v.Text, a => a.MapFrom(e => e.Text))
                .ForMember(v => v.IsDeleted, a => a.MapFrom(e => e.IsDeleted));

            // Question and QuestionViewModel
            CreateMap<Question, QuestionViewModel>()
                .ForMember(q => q.Id, v => v.MapFrom(m => m.Id))
                .ForMember(q => q.Text, v => v.MapFrom(m => m.Text))
                .ForMember(q => q.Answers, v => v.MapFrom(m => m.Answers))
                .ForMember(q => q.RightAnswer, v => v.MapFrom(m => m.RightAnswer))
                .ForMember(q => q.IsDeleted, v => v.MapFrom(m => m.IsDeleted));
            CreateMap<QuestionViewModel, Question>()
                .ForMember(v => v.Id, q => q.MapFrom(e => e.Id))
                .ForMember(v => v.Text, q => q.MapFrom(e => e.Text))
                .ForMember(v => v.Answers, q => q.MapFrom(e => e.Answers))
                .ForMember(v => v.RightAnswer, q => q.MapFrom(e => e.RightAnswer))
                .ForMember(v => v.IsDeleted, q => q.MapFrom(e => e.IsDeleted));

            // User and UserViewModel
            CreateMap<User, UserViewModel>()
                .ForMember(u => u.Id, v => v.MapFrom(m => m.Id))
                .ForMember(u => u.Name, v => v.MapFrom(m => m.Name))
                .ForMember(u => u.Email, v => v.MapFrom(m => m.Email))
                .ForMember(u => u.IsDeleted, v => v.MapFrom(m => m.IsDeleted));
            CreateMap<UserViewModel, User>()
                .ForMember(v => v.Id, u => u.MapFrom(e => e.Id))
                .ForMember(v => v.Name, u => u.MapFrom(e => e.Name))
                .ForMember(v => v.Email, u => u.MapFrom(e => e.Email))
                .ForMember(v => v.IsDeleted, u => u.MapFrom(e => e.IsDeleted));
        }
    }
}
