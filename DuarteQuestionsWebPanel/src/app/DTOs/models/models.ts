export interface CreateUserCommand {
  name: string;
  email: string;
  confirmedEmail: string;
  password: string;
  confirmedPassword: string;
}

export interface UpdateUserCommand {
  id: number;
  name: string;
  email: string;
  confirmedEmail: string;
  password: string;
  confirmedPassword: string;
}

export interface GetUserListQuery {
  keyword: string;
  getAll: boolean;
}

export interface LogInCommand {
  username: string;
  password: string;
}

export interface RestoreUserCommand {
  username: string;
  password: string;
  confirmedPassword: string;
}

export interface ChangePasswordCommand {
  username: string;
  currentPassword: string;
  newPassword: string;
  confirmedPassword: string;
}

export interface UserViewModel {
  id: number;
  name: string;
  email: string;
  isDeleted: boolean;
}

export interface CreateAnswerCommand {
  text: string;
}

export interface AnswerCreatedDTO {
  id: number;
  text: string;
}

export interface UpdateAnswerCommand {
  id: number;
  text: string;
}

export interface GetAnswerListQuery {
  keyword: string;
  getAll: boolean;
}

export interface AnswerViewModel {
  id: number;
  text: string;
  isDeleted: boolean;
}

export interface CreateQuestionCommand {
  text: string;
  answers: number[];
  rightAnswer: number;
}

export interface UpdateQuestionCommand {
  id: number;
  text: string;
  answers: number[];
  rightAnswer: number;
}

export interface GetQuestionListQuery {
  keyword: string;
  getAll: boolean;
}

export interface QuestionViewModel {
  id: number;
  text: string;
  answers: any[]; // Answer type
  rightAnswer: any; // Answer type
  isDeleted: boolean;
}