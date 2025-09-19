using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

using static Core.Program;
using Core;

namespace Core.DataProcess
{

    #region 25.04.05 LYK 테이블 구조 정의용 클래스

    /// <summary>
    /// 25.04.05 LYK 테이블 구조를 정의하는 클래스
    /// </summary>
    public class TableDefinition
    {
        public string TableName { get; set; }

        /// <summary>
        /// 25.04.05 LYK 컬럼명과 타입의 딕셔너리 (예: "Name" : "TEXT NOT NULL")
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }
    }

    #endregion

    /// <summary>
    /// 25.04.05 LYK SQLite 데이터 매니저 추상 클래스 (제네릭)
    /// </summary>
    /// <typeparam name="T">관리할 데이터 타입</typeparam>
    public abstract class CDataManager<T>
    {
        protected SQLiteConnection Connection;
        protected SQLiteTransaction _transaction;

        /// <summary>
        /// 25.04.05 LYK 생성자: DB 경로로 연결 생성 및 테이블 생성
        /// </summary>
        public CDataManager(string databasePath)
        {
            Connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            Connection.Open();
            CreateTables(GetTableDefinitions()); // 테이블 정의에 따라 생성
        }

        /// <summary>
        /// 테이블 생성 (여러 개 가능)
        /// </summary>
        protected void CreateTables(List<TableDefinition> definitions)
        {
            foreach (var def in definitions)
            {
                var columns = string.Join(", ",
                    def.Columns.Select(c => $"{c.Key} {c.Value}"));

                string query = $"CREATE TABLE IF NOT EXISTS {def.TableName} ({columns});";
                ExecuteNonQuery(query);
            }
        }

        /// <summary>
        /// 25.04.05 LYK 테이블 정의를 제공하는 추상 메서드 (파생 클래스에서 구현)
        /// </summary>
        protected abstract List<TableDefinition> GetTableDefinitions();

        /// <summary>
        /// 25.04.05 LYK 단순 쿼리 실행
        /// </summary>
        protected void ExecuteNonQuery(string query)
        {
            using (var command = new SQLiteCommand(query, Connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 25.04.05 LYK 단일 값을 반환하는 쿼리 실행 (예: COUNT, MAX 등)
        /// </summary>
        protected TVal ExecuteScalar<TVal>(string query)
        {
            using (var command = new SQLiteCommand(query, Connection))
            {
                object result = command.ExecuteScalar();
                return (TVal)Convert.ChangeType(result, typeof(TVal));
            }
        }

        /// <summary>
        /// 25.04.05 LYK DB 연결 종료
        /// </summary>
        public void CloseConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        #region 25.04.05 LYK  트랜잭션 관련

        /// <summary>
        /// 25.04.05 LYK BeginTransaction
        /// 트랜잭션 시작 이후의 DB 작업은 commit() 또는 rollback() 전까지 하나의 작업 단위로 묶음
        /// 예: 여러 INSERT를 하나의 트랜잭션으로 처리할 때 사용.
        /// </summary>
        public void BeginTransaction()
        {
            _transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// 25.04.05 LYK Commit 함수 
        /// 현재 트랜잭션을 커밋
        /// 지금까지 실행한 DB 작업들이 모두 최종 저장.
        /// </summary>
        public void Commit()
        {
            _transaction?.Commit();  // null이면 무시
            _transaction = null;
        }

        /// <summary>
        /// 25.04.05 LYK Rollback 함수
        /// 현재 트랜잭션을 롤백
        /// 트랜잭션 시작 이후 실행된 모든 DB 작업을 되돌림
        /// </summary>
        public void Rollback()
        {
            _transaction?.Rollback();  // null이면 무시
            _transaction = null;
        }

        #endregion

        #region 범용 유틸 메서드

        /// <summary>
        /// 25.04.05 LYK 특정 조건의 데이터가 존재하는지 확인
        /// </summary>
        public bool Exists(string tableName, string whereClause)
        {
            return Count(tableName, whereClause) > 0;
        }

        /// <summary>
        /// 25.04.05 LYK 특정 조건의 데이터 수를 반환
        /// </summary>
        public int Count(string tableName, string whereClause = "")
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
                query += $" WHERE {whereClause}";

            return ExecuteScalar<int>(query);
        }

        #endregion

        #region 추상 메서드 정의 (파생 클래스에서 구현)

        /// <summary>
        /// 25.04.05 LYK 여러 개의 데이터를 한 번에 삽입
        /// </summary>
        public abstract void InsertData(IEnumerable<T> dataList);

        /// <summary>
        /// 25.04.05 LYK 조건에 따른 데이터 조회
        /// </summary>
        public abstract List<T> SearchData(string whereClause = "");

        /// <summary>
        /// 25.04.05 LYK 조건에 따른 데이터 삭제
        /// </summary>
        public abstract void DeleteData(string whereClause = "");

        /// <summary>
        /// 25.04.05 LYK 특정 데이터 갱신
        /// </summary>
        public abstract void UpdateData(T item, string whereClause);

        #endregion
    }
}
