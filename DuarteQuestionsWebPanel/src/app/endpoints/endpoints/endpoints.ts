export class EndPoints {
  private static API_USER = "api/user";
  public static API_USER_CREATE = EndPoints.API_USER + "/create-user";
  public static API_USER_UPDATE = EndPoints.API_USER + "/update-user";
  public static API_USER_DELETE = EndPoints.API_USER + "/delete-user";
  public static API_USER_GET_LIST = EndPoints.API_USER + "/get-user-list";
  public static API_USER_GET = EndPoints.API_USER + "/get-user";
  public static API_USER_LOGIN = EndPoints.API_USER + "/login";
  public static API_USER_RESTORE = EndPoints.API_USER + "/restore-user";
  public static API_USER_CHANGE_PASSWORD = EndPoints.API_USER + "/change-user-password";

  public static API_ANSWER = "api/answer";
  public static API_ANSWER_CREATE = EndPoints.API_ANSWER + "/create-answer";
  public static API_ANSWER_UPDATE = EndPoints.API_ANSWER + "/update-answer";
  public static API_ANSWER_DELETE = EndPoints.API_ANSWER + "/delete-answer";
  public static API_ANSWER_GET_LIST = EndPoints.API_ANSWER + "/get-answer-list";
  public static API_ANSWER_GET = EndPoints.API_ANSWER + "/get-answer";
  public static API_ANSWER_RESTORE_ALL = EndPoints.API_ANSWER + "/restore-all";

  public static API_QUESTION = "api/question";
  public static API_QUESTION_CREATE = EndPoints.API_QUESTION + "/create-question";
  public static API_QUESTION_UPDATE = EndPoints.API_QUESTION + "/update-question";
  public static API_QUESTION_DELETE = EndPoints.API_QUESTION + "/delete-question";
  public static API_QUESTION_GET_LIST = EndPoints.API_QUESTION + "/get-question-list";
  public static API_QUESTION_GET = EndPoints.API_QUESTION + "/get-question";
  public static API_QUESTION_RESTORE_ALL = EndPoints.API_QUESTION + "/restore-all";
}