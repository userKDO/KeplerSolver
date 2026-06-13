using System.Text;
using System.Text.Json;

namespace logger
{
	public class ERROR
	{
		public string? log_msg { get; set; }

		public ERROR(string? Log_msg)
		{
			log_msg = Log_msg;
		}
	}

	public class WARNING
	{
		public string? log_msg { get; set; }

		public WARNING(string? Log_msg)
		{
			log_msg = Log_msg;
		}
	}

	public class DOUBLE_DATA
	{
		public string? data { get; set; }

		public DOUBLE_DATA(string? Data)
		{
			data = Data;
		}
	}

	public class INFO
	{
		public string? log_msg { get; set; }
		public INFO(string? Log_msg)
		{
			log_msg = Log_msg;
		}
	}
	
	
	public class TESTPASSED
	{
		public string? log_msg{ get; set; }
		
		public TESTPASSED(string? Log_msg)
		{
			log_msg = Log_msg;
		}
	}
	
	public class TESTFAILED
	{
		public string? log_msg{ get; set; }
		
		public TESTFAILED(string? Log_msg)
		{
			log_msg = Log_msg;
		}
	}

	public class SimpleLogger
	{
		private async Task LogInfoInternal(string log_msg)
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					INFO log = new INFO($"[INFO] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {log_msg}");
					await JsonSerializer.SerializeAsync<INFO>(fs, log);
					await fs.WriteAsync(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data has been saved to file");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occured: {ex.Message}");
			}
		}
		
		private static void LogTestPassed(string log_msg)
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					TESTPASSED log = new TESTPASSED($"[TEST PASSED] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {log_msg}");
					JsonSerializer.Serialize<TESTPASSED>(fs, log);
					fs.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data about test has been saved to file");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		private async Task LogErrorInternal(string log_msg)
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					ERROR log = new ERROR($"[ERROR] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {log_msg}");
					await JsonSerializer.SerializeAsync<ERROR>(fs, log);
					await fs.WriteAsync(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data has been saved to file");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occured: {ex.Message}");
			}
		}
		
		private static void LogTestFailed(string log_msg)
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					TESTFAILED log = new TESTFAILED($"[TEST FAILED] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {log_msg}");
					JsonSerializer.Serialize<TESTFAILED>(fs, log);
					fs.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data about test has been saved to file");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occuted: {ex.Message}");
			}
		}
		
		private async Task LogWarningInternal(string log_msg)
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					WARNING log = new WARNING($"[WARNING] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {log_msg}");
					await JsonSerializer.SerializeAsync<WARNING>(fs, log);
					await fs.WriteAsync(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data has been saved to file");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"An error occured: {ex.Message}");
			}
		}

		private async Task LogDataInternal(string data) // logger for data in double
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.Append))
				{
					DOUBLE_DATA log = new DOUBLE_DATA($"[DATA] ({DateTime.Now:yyyy-MM-dd HH:mm:ss}): {data}");
					await JsonSerializer.SerializeAsync<DOUBLE_DATA>(fs, log);
					await fs.WriteAsync(Encoding.UTF8.GetBytes(Environment.NewLine));
					Console.WriteLine("Data has been saved to file");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"An error occured: {ex.Message}");
			}
		}

		private static void ReadLogs()
		{
			try
			{
				using (FileStream fs = new FileStream("logs.ndjson", FileMode.OpenOrCreate))
				{
					if (fs.Length > 0)
					{
						List<string> logs = new List<string>();
						using (StreamReader reader = new StreamReader(fs))
						{
							string line;
							while ((line = reader.ReadLine()) != null)
							{
								logs.Add(line);
							}
						}
                
						foreach (var log in logs)
						{
							Console.WriteLine(log);
						}
					}
					else
					{
						Console.WriteLine("Log file is empty");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occured: {ex.Message}");
			}
		}

		public async Task LogInfo(string message)
		{
			await LogInfoInternal(message);
		}

		public async Task LogError(string message)
		{
			await LogErrorInternal(message);
		}

		public async Task LogWarning(string message)
		{
			await LogWarningInternal(message);
		}

		public async Task LogData(string message)
		{
			await LogDataInternal(message);
		}

		public void LogRead()
		{
			ReadLogs();
		}
		
		public void logTestPassed(string message)
		{
			LogTestPassed(message);
		}
		
		public void logTestFailed(string message)
		{
			LogTestFailed(message);
		}

	}
	
}
