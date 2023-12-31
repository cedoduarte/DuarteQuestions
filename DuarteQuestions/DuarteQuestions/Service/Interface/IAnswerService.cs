﻿using DuarteQuestions.CQRS.Answers.Command.CreateAnswer;
using DuarteQuestions.CQRS.Answers.Command.UpdateAnswer;
using DuarteQuestions.CQRS.Answers.Query.GetAnswerList;
using DuarteQuestions.CQRS.Answers.ViewModel;
using DuarteQuestions.DTOs;

namespace DuarteQuestions.Service.Interface
{
    public interface IAnswerService
    {
        Task<AnswerCreatedDTO> CreateAnswer(CreateAnswerCommand command);
        Task<bool> UpdateAnswer(UpdateAnswerCommand command);
        Task<bool> DeleteAnswer(int id);
        Task<IEnumerable<AnswerViewModel>> GetAnswerList(GetAnswerListQuery query);
        Task<AnswerViewModel> GetAnswerById(int id);
        Task<bool> RestoreAll();
    }
}
