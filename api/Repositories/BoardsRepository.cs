using System.ComponentModel.Design;
using System.Data;
using border.api.Models;
using border.api.Services;
using Dapper;
using Microsoft.AspNetCore.SignalR;

namespace border.api.Repositories
{
    public class BoardsRepository
    {
        IDatabaseHelper _dbHelper;
        protected virtual string QueryUserBoardsSQL
        {
            get
            {
                return $@"select 
                        b.id as {nameof(Board.Id)}, 
                        b.name as {nameof(Board.Name)}, 
                        b.created_by as {nameof(Board.CreatedBy)}, 
                        b.created_time as {nameof(Board.CreatedTime)}  
                    from boards as b 
                    inner join user_boards as ub on ub.board_id = b.id and ub.user_id = @UserId
                    order by {nameof(Board.Name)}";
            }
        }

        protected virtual string QueryBoardSQL
        {
            get
            {
                return $@"select 
                        b.id as {nameof(Board.Id)}, 
                        b.name as {nameof(Board.Name)}, 
                        b.created_by as {nameof(Board.CreatedBy)}, 
                        b.created_time as {nameof(Board.CreatedTime)}  
                    from boards as b 
                    where b.id = @Id";
            }
        }

        protected virtual string InsertBoardSQL
        {
            get
            {
                return $@"insert into boards(name, created_by, created_time) values (@Name, @CreatedBy, @CreatedTime)";
            }
        }
        public BoardsRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<IEnumerable<Board>> GetUserBoards(string userId)
        {
            using (IDbConnection connection = _dbHelper.CreateConnection())
            {
                connection.Open();
                var boards = await connection.QueryAsync<Board>(QueryUserBoardsSQL, new { UserId = userId });
                return boards;
            }
        }
        public async Task<Board?> GetBoard(long boardId)
        {
            using (IDbConnection connection = _dbHelper.CreateConnection())
            {
                connection.Open();
                var board = await connection.QueryFirstOrDefaultAsync<Board>(QueryBoardSQL, new { Id = boardId });
                return board;
            }
        }
        public async Task
    }
}