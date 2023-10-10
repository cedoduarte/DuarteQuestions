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