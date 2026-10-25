namespace SimpleChat.Services;

public interface IUserStatsService
{
    int GetTotalCount();
    string GetLastUserName();
}