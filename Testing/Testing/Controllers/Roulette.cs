using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Testing.DbConnect;
using Testing.Model;
using Testing.Models;

namespace Testing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Roulette : ControllerBase
    {


        private readonly ILogger<Roulette> _logger;
        public SqliteConnect _sqlliteconn;
        Task<int> result;
        ResponseMessage message;


        public Roulette(ILogger<Roulette> logger, SqliteConnect Sqliteconn)
        {
            _logger = logger;
            _sqlliteconn = Sqliteconn;
        }


        [HttpPost("PlaceBet")]
        public async Task<ResponseMessage> PlaceBet(int number, string colour, int EONumber)
        {

            string Proc = @"
	                            INSERT INTO PLACED_BETS
	                            (Id, Chosennumber , Colour,  EONumber )
	                            VALUES
	                            (@id,@number, @colour, @EONumber);";

            try
            {

                if (number < 0 || number > 36)
                {
                    message =
                        new ResponseMessage()
                        {
                            isSuccessful = false,
                            message = "Number should be between 0 to 36"
                        };

                    return message;
                }
                else
                {
                    String idkey = Guid.NewGuid().ToString();
                    using (IDbConnection database = _sqlliteconn.Connect())
                    {
                        object parameters = new
                        {
                            id = idkey,
                            number = number,
                            colour = colour,
                            EONumber = EONumber
                        };
                        result = database.ExecuteAsync(Proc, parameters);
                        var x = result.IsCompleted.ToString();
                        if (result.IsCompleted.ToString() == "True")
                        {
                            message =
                                new ResponseMessage()
                                {
                                    isSuccessful = false,
                                    message = "Your bet was successfully placed, once rolled, please use the key token to calculate your winnings. Key Token:" + idkey
                                };

                            return message;
                        }
                    }

                    return null;
                }

            }


            catch (Exception e)
            {
                _logger.LogError(e, "Place Bet", "Input "+number+","+colour+","+EONumber);
                message =
                               new ResponseMessage()
                               {
                                   isSuccessful = false,
                                   message = "An exception occured"
                               };
                return message;
            }
        }
        [HttpPost("Spin")]
        public async Task<ResponseMessage> Spin()
        {
            string idkey = Guid.NewGuid().ToString();
            Random rnd = new Random();
            int number = rnd.Next(0, 36);  // creates a number between 1 and 12
            int EONumber = rnd.Next(0, 36);   // creates a number between 1 and 6
            List<string> list = new List<string> { "Black", "Red" };
            string colour = list[rnd.Next(list.Count)];
            string spinproc = @"
	                            INSERT INTO SPINNED_BETS
	                            (BetId, Chosennumber , Colour,  EONumber )
	                            VALUES
	                            (@id,@number, @colour, @EONumber);";
            string Update = @"
	                           UPDATE PLACED_BETS
                               SET BetID = @id, Expired = @Exp
                               WHERE BetID is null;";
            object parameters = new
            {
                id = idkey,
                number = number,
                colour = colour,
                EONumber = EONumber
            };
            object updpar = new
            {
                id = idkey,
                Exp = "Y"
            };

            using (IDbConnection database = _sqlliteconn.Connect())
            {
                try
                {
                    result = database.ExecuteAsync(spinproc, parameters);
                    Task<int> result2 = database.ExecuteAsync(Update, updpar);
                    var x = result.IsCompleted.ToString();
                    var y = result2.IsCompleted.ToString();
                    if (result.IsCompleted.ToString() == "True")
                    {
                        message =
                            new ResponseMessage()
                            {
                                isSuccessful = false,
                                message = "Winning numbers are Selected Number: " + number + " , Winning colour " + colour + " ,Odd Even number " + EONumber
                            };
                    }
                    return message;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Spin", null);
                    message =
                           new ResponseMessage()
                           {
                               isSuccessful = false,
                               message = "An error occured"
                           };
                    return message;
                }
            }

        }
        [HttpGet("ViewPreviousSpins")]
        public async Task<List<SpinsView>> ViewPreviousSpins()
        {
           /* select BetId,
             Chosennumber,
             Colour,
             EONumber */
            string select_spins = @"select BetId,
             Chosennumber,
             Colour,
             EONumber  from SPINNED_BETS";

            string sVA = @"select *  from PLACED_BETS";
            List<Task<SpinsView>> nList = new List<Task<SpinsView>>();
            List<SpinsView> Data = new List<SpinsView>();
            try
            {
                List<SpinsView> result = new List<SpinsView>();
                using (IDbConnection database = _sqlliteconn.Connect())
                {
                    IEnumerable<SpinsView> dataTable =  await database.QueryAsync<SpinsView>(select_spins);
                    var px = database.Query(sVA);
                     Data = dataTable.ToList();
                }
              
                return Data;
            }catch(Exception e)
            {
                _logger.LogError(e, "ViewPreviousViews", null);
                return null;
            }
        }
    }
}

