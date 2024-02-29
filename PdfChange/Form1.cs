using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BLL;
using BLL.Interfaces;
using DAL;
using EmailSender;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace PdfChange
{
	public partial class Form1 : Form
	{
		private string folderPath;
		private string MailFolderPath;
		private EmployeeManager employeeManager;
		private SendlingEmail sendlingEmail;
		public Form1()
		{
			InitializeComponent();
			Idtextnum.Minimum = 0;
			Idtextnum.Maximum = 100000;
			employeeManager = new EmployeeManager(new EmployeeRepository());
			sendlingEmail = new SendlingEmail(new EmialSender());
			tabPage1.Text = "Pdf name changer";
			tabPage2.Text = "CSV manager";
			tabPage3.Text = "Sender";
			Reload();
		}

		private void Reload()
		{
			List<Employee> employees = employeeManager.GetEmployees();
			employees = employees.OrderBy(emp => emp.Id).ToList();
			Datalistbx.DataSource = employees;
		}
		private bool CsvCheck(int id)
		{
			if (employeeManager.GetEmployeeById(id) != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		private bool Check()
		{
			if (DocumentNameTxtbx.Text != string.Empty && MapNameTxtbx.Text != string.Empty && folderPath != null)
			{
				return true;
			}
			else
			{
				return false;
			}

		}
		private bool Check2()
		{
			if (Titletxtbx.Text != string.Empty && MailFolderTxtbx.Text != string.Empty)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		private string ReturnName(string text)
		{

			string pattern = @"(?:De heer|Mevrouw)\s*(.*)";

			Match match = Regex.Match(text, pattern);


			if (match.Success)
			{
				return match.Groups[1].Value + " " + match.Groups[2].Value;
			}
			else
			{
				return "0";
			}
		}

		private string ReturnNumber(string text)
		{

			string pattern = @"Werknemer\s+(\d+(\s+\d+)*)";

			Match match = Regex.Match(text, pattern);

			if (match.Success)
			{
				string extractedText = match.Groups[1].Value;
				return extractedText;
			}
			else
			{
				return "0";
			}
		}
		private string ReturnText(string file)
		{
			StringBuilder sb = new StringBuilder();

			using (PdfReader reader = new PdfReader(file))
			{
				for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
				{
					ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
					string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);
					text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
					sb.Append(text);
				}
			}
			return sb.ToString();
		}
		private string CreateMap()
		{

			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


			string newFolderName = MapNameTxtbx.Text;

			string newFolderPath = System.IO.Path.Combine(documentsPath, newFolderName);

			if (!Directory.Exists(newFolderPath))
			{
				Directory.CreateDirectory(newFolderPath);
				return newFolderPath;
			}
			else
			{
				MessageBox.Show("Folder already exists.");
				return null;
			}
		}





		private void ConvertBtn_Click(object sender, EventArgs e)
		{
			if (Check() == false)
			{
				MessageBox.Show("Je mist iets vul het nog in");
			}
			else
			{
				string MapPath = CreateMap();
				if (MapPath != null)
				{
					foreach (string pdfFilePath in Directory.GetFiles(folderPath, "*.pdf"))
					{


						string PdfText = ReturnText(pdfFilePath);
						string number = ReturnNumber(PdfText);
						string name = ReturnName(PdfText);

						string sourceFilePath = pdfFilePath;
						string targetFolderPath = MapPath;

						if (targetFolderPath != null)
						{

							string targetFileName = number + " " + DocumentNameTxtbx.Text + " " + name + ".pdf";
							string targetFilePath = System.IO.Path.Combine(targetFolderPath, targetFileName);

							try
							{
								File.Copy(sourceFilePath, targetFilePath);

							}
							catch (Exception ex)
							{
								MessageBox.Show("An error occurred: " + ex.Message);
							}
						}
					}
					MessageBox.Show("Gelukt");
				}
			}


		}

		private void SelectBtn_Click(object sender, EventArgs e)
		{
			using (var folderDialog = new FolderBrowserDialog())
			{
				if (folderDialog.ShowDialog() == DialogResult.OK)
				{
					string selectedFolderPath = folderDialog.SelectedPath;
					folderPath = selectedFolderPath;
					PathTxtbx.Text = selectedFolderPath;
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{


		}

		private void Datalistbx_SelectedIndexChanged(object sender, EventArgs e)
		{
			var employee = Datalistbx.SelectedItem as Employee;
			Idtextnum.Value = employee.Id;
			MailTxtbx.Text = employee.Email.ToString();
		}

		private void AddBtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("wil je dit toevoegen?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (CsvCheck((int)Idtextnum.Value) == false)
			{
				MessageBox.Show("er is al een werknemer met dit id");
			}
			else
			{
				if (result == DialogResult.Yes)
				{
					Employee employee = new Employee((int)Idtextnum.Value, MailTxtbx.Text);
					employeeManager.AddEmployee(employee);
				}
				if (result == DialogResult.No)
				{

				}
				Reload();
			}

		}

		private void EditBtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Wil je dit wijzigen", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


			if (CsvCheck((int)Idtextnum.Value) == true)
			{
				MessageBox.Show("er is geen werknemer met dit id");
			}
			else
			{
				if (result == DialogResult.Yes)
				{
					Employee employee = new Employee((int)Idtextnum.Value, MailTxtbx.Text);
					employeeManager.EditEmployee(employee);
				}
				if (result == DialogResult.No)
				{

				}
				Reload();
			}
		}

		private void Removebtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Wil je dit verwijderen?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (CsvCheck((int)Idtextnum.Value) == true)
			{
				MessageBox.Show("er is geen werknemer met dit id");
			}
			else
			{
				if (result == DialogResult.Yes)
				{
					employeeManager.RemoveEmployee((int)Idtextnum.Value);
				}
				if (result == DialogResult.No)
				{

				}
				Reload();
			}
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void SelectMailPdfBtn_Click(object sender, EventArgs e)
		{
			using (var folderDialog = new FolderBrowserDialog())
			{
				if (folderDialog.ShowDialog() == DialogResult.OK)
				{
					string selectedFolderPath = folderDialog.SelectedPath;
					MailFolderPath = selectedFolderPath;
					MailFolderTxtbx.Text = selectedFolderPath;
				}
			}
		}

		private async void SendBtn_Click(object sender, EventArgs e)
		{
			List<string> notSend = Directory.GetFiles(MailFolderPath, "*.pdf").ToList();

			if (Check2())
			{
				try
				{
					foreach (var pdfFilePath in notSend)
					{
						string PdfText = ReturnText(pdfFilePath);
						string number = ReturnNumber(PdfText);
						string name = ReturnName(PdfText);

						Employee targetEmployee = employeeManager.GetEmployees().FirstOrDefault(x => x.Id == Convert.ToInt32(ReturnNumber(ReturnText(pdfFilePath))));
						if (targetEmployee != null)
						{
							Attachment attachment = new Attachment(pdfFilePath, MediaTypeNames.Application.Pdf);
							var mail = targetEmployee.Email;
							var title = Titletxtbx.Text;
							var message = string.IsNullOrEmpty(MessageTextbx.Text) ? "Loonstrook" : MessageTextbx.Text;
							var FileName = System.IO.Path.GetFileName(pdfFilePath);

							if (ConfirmDialogCheckBox.Checked)
							{
								DialogResult result = DialogResult.None;

								// Invoke UI operation to show MessageBox on the UI thread
								FailedListbx.Invoke(new Action(() =>
								{
									result = MessageBox.Show($"Wil je deze pdf {FileName} naar deze mail sturen {mail}", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
								}));

								if (result == DialogResult.Yes)
								{
									try
									{
										bool success = await sendlingEmail.SendEmail(mail, title, message, attachment);
										await Task.Delay(TimeSpan.FromSeconds(30));

										if (!success)
										{
											FailedListbx.Invoke(new Action(() =>
											{
												FailedListbx.Items.Add($"{number} - {name}");
											}));
										}
									}
									catch (Exception ex)
									{
										FailedListbx.Invoke(new Action(() =>
										{
											FailedListbx.Items.Add($"{number} - {name}");
										}));
									}
								}
							}
							else
							{
								try
								{
									bool success = await sendlingEmail.SendEmail(mail, title, message, attachment);
									await Task.Delay(TimeSpan.FromSeconds(30));

									if (!success)
									{
										FailedListbx.Invoke(new Action(() =>
										{
											FailedListbx.Items.Add($"{number} - {name}");
										}));
									}
								}
								catch (Exception ex)
								{
									FailedListbx.Invoke(new Action(() =>
									{
										FailedListbx.Items.Add($"{number} - {name}");
									}));
								}
							}
						}
						else
						{
							FailedListbx.Invoke(new Action(() =>
							{
								FailedListbx.Items.Add($"{number} - {name}");
							}));
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
			else
			{
				MessageBox.Show("Je mist iets, vul het nog in");
			}
		}




		private void tabPage1_Click(object sender, EventArgs e)
		{
		}


	}
}
