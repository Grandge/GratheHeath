using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GratheHeath.Models;
using GratheHeath.ViewModels;

/// <summary>
/// メモ：今回はリテラルや変数名にマルチバイト文字を多用してみた。
/// 気持ち悪いので今までなんとなく使うのを避けていたけれど、これはこれで悪くない
/// enmuの要素を日本語にするのは分かりやすくなってよかった
/// </summary>

namespace GratheHeath
{


    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //CSVファイル
        private CsvData _Csv_ファイル一覧;
        private CsvData _Csv_iCD_タスク一覧;
        private CsvData _Csv_iCD_タスクＸスキル対応表;
        private CsvData _Csv_iCD_スキル一覧_スキル項目;
        private CsvData _Csv_iCD_スキル一覧_知識項目;
        private CsvData _Csv_iCD_タスクプロフィール一覧;
        private CsvData _Csv_iCD_タスクプロフィールＸタスク対応表;
        public VM選択項目 vmタスクプロフィール選択用 { get; set; }
        public VM選択項目 vmタスク一覧選択用 { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            //コマンドライン引数を配列で取得する
            String vertext = システムバージョン取得();
            Message(String.Format("iCD Data viewer 'Grathe Heath'  Ver{0} \r\n", vertext.ToString()));
            string[] cmds = System.Environment.GetCommandLineArgs();
            Title = "Grath Heath " + vertext;

            _Csv_ファイル一覧 = new CsvData("ファイル一覧.csv");
            List<string> listファイル = _Csv_ファイル一覧.Rows.Where(line => !line[0].StartsWith("#")).Select(line => line[0]).ToList();
            if( listファイル.Count() == 6)
            {

                _Csv_iCD_タスク一覧 = new CsvData(listファイル[0]);
                _Csv_iCD_タスクＸスキル対応表 = new CsvData(listファイル[1]);
                _Csv_iCD_スキル一覧_スキル項目 = new CsvData(listファイル[2]);
                _Csv_iCD_スキル一覧_知識項目 = new CsvData(listファイル[3]);
                _Csv_iCD_タスクプロフィール一覧 = new CsvData(listファイル[4]);
                _Csv_iCD_タスクプロフィールＸタスク対応表 = new CsvData(listファイル[5]);

            }
            else
            {
                Message(string.Format("データファイルが読み込めませんでした。ファイル一覧.csvの内容をご確認ください。\n"));
            }


            vmタスクプロフィール選択用 = new VM選択項目();
            vmタスク一覧選択用 = new VM選択項目();
            _Csv_iCD_タスクプロフィール一覧.Rows.ForEach( p => 
            {
                vmタスクプロフィール選択用.ListItem.Add(new C選択アイテム(
                    p[(int)Eタスクプロフィール一覧見出し.タスクプロフィール],
                    p[(int)Eタスクプロフィール一覧見出し.タスクプロフィールコード]));
            });
            //一行目は見出し行なので削除
            if(vmタスクプロフィール選択用.ListItem.Count() > 0)
            {
                vmタスクプロフィール選択用.ListItem.RemoveAt(0);

            }
            listBoxタスクプロフィール選択.ItemsSource = vmタスクプロフィール選択用.ListItem;

        }

        private string システムバージョン取得()
        {
            //自分自身のAssemblyを取得
            System.Reflection.Assembly asm =
                System.Reflection.Assembly.GetExecutingAssembly();
            //バージョンの取得
            System.Version ver = asm.GetName().Version;
            return ver.ToString();
        }
        /// <summary>
        /// メッセージエリアへのログ出力とカーソル移動
        /// </summary>
        /// <param name="message"></param>
        private void Message(string message)
        {
            textBoxMessages.AppendText(message);
            textBoxMessages.SelectionStart = textBoxMessages.Text.Length;

        }

        private void スキル検索()
        {
            //表示領域をクリア
            textBoxMessages.Text = "";
            //タスク小分類コードに一致するタスクを検索
            string code = textBoxTaskCode.Text;

            List<string> skillsText = new List<string>();

            //タスクコードに関連するスキルの一覧作成
            skillsText = スキル項目検索(code);
            skillsText.ForEach(t => { Message(t); });

        }
        private List<string> スキル項目検索(string code)
        {
            //タスクの小分類名を表示
            List<string> result = new List<string>();
            List<string[]> taskCodeList = _Csv_iCD_タスク一覧.Rows.Where(task => task[(int)Eタスク一覧見出し.タスク小分類コード] == code).ToList();
            if (taskCodeList.Count() > 0)
            {
                result.Add("-----------------------------------\n");
                result.Add(string.Format("{0}\n", (taskCodeList[0])[(int)Eタスク一覧見出し.タスク小分類]));
                result.Add("-----------------------------------\n");
            }

            //検索したタスクの小分類コードと一致するスキル集合のスキル項目コードを（重複無しで）取得する
            List<string> skillCodeList = new List<string>();
            taskCodeList.ForEach(x =>
            {
                skillCodeList.AddRange(
                   _Csv_iCD_タスクＸスキル対応表.Rows.Where(
                       r => r[(int)EタスクXスキル対応表見出し.タスク小分類コード] == x[(int)Eタスク一覧見出し.タスク小分類コード]).Select(
                        r => r[(int)EタスクXスキル対応表見出し.スキル項目コード]).OrderBy(c => c).Distinct().ToList());
            });

            //---------------------------------------------------
            //評価項目コードに一致するスキル集合を取得する
            List<string[]> skillList = new List<string[]>();
            skillCodeList.ForEach(skillCode =>
            {
                skillList.AddRange(
                    _Csv_iCD_スキル一覧_スキル項目.Rows.Where(skill => skill[(int)Eスキル一覧_スキル項目見出し.スキル項目コード] == skillCode).ToList()
                );

            });

            //個々のスキル名を昇順にソートして重複を除去してから表示
            var skillNameList = skillList.Select(s2 => s2[(int)Eスキル一覧_スキル項目見出し.スキル項目]).OrderBy(e2 => e2).Distinct().ToList();
            if (skillNameList.Count() > 0)
            {
                result.Add("----スキル項目-----\n");
                skillNameList.ForEach(s =>
                {
                    result.Add(string.Format("{0}\n", s));
                });
            }
            //---------------------------------------------------
            //評価項目コードに一致するスキル集合を取得する
            List<string[]> skillList2 = new List<string[]>();
            skillCodeList.ForEach(skillCode =>
            {
                skillList2.AddRange(
                    _Csv_iCD_スキル一覧_知識項目.Rows.Where(skill => skill[(int)Eスキル一覧_知識項目見出し.スキル項目コード] == skillCode).ToList()
                );

            });

            //個々のスキル名を昇順にソートして重複を除去してから表示
            var skillNameList2 = skillList.Select(s2 => s2[(int)Eスキル一覧_知識項目見出し.知識項目]).OrderBy(e2 => e2).Distinct().ToList();
            if (skillNameList2.Count() > 0)
            {
                result.Add("----知識項目-----\n");
                skillNameList2.ForEach(s =>
                {
                    result.Add(string.Format("{0}\n", s));
                });
            }
            return result;
        }
        private List<string> 評価項目検索(string code)
        {
            //タスクの小分類名を表示
            List<string> result = new List<string>();
            List<string[]> taskCodeList = _Csv_iCD_タスク一覧.Rows.Where(task => task[(int)Eタスク一覧見出し.タスク小分類コード] == code).ToList();
            if (taskCodeList.Count() > 0)
            {
                taskCodeList.ForEach(t => 
                {
                    result.Add("*" + string.Format("{0}\n", t[(int)Eタスク一覧見出し.評価項目]));
                });
            }
            return result;
        }

        private void タスクプロフィール選択_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            C選択アイテム item = ((sender as System.Windows.Controls.ListBox).SelectedItem as C選択アイテム);
            textBoxタスクプロフィール.Text = item.Code;
            vmタスク一覧選択用 = new VM選択項目();

            var listTasks = _Csv_iCD_タスクプロフィールＸタスク対応表.Rows.Where(prcode =>
                prcode[(int)EタスクプロフィールＸタスク対応表見出し.タスクプロフィールコード] == item.Code)
                .OrderBy( i => i[(int)EタスクプロフィールＸタスク対応表見出し.タスク小分類コード])
                .Distinct().ToList();
            if(listTasks.Count() > 0)
            {
                listTasks.ForEach(task =>
                {
                    string[] taskName = _Csv_iCD_タスク一覧.Rows.Where(t => t[(int)Eタスク一覧見出し.タスク小分類コード] == task[(int)EタスクプロフィールＸタスク対応表見出し.タスク小分類コード]).First();
                    string name = taskName[(int)Eタスク一覧見出し.タスク小分類コード] + " : " + taskName[(int)Eタスク一覧見出し.タスク小分類];
                    vmタスク一覧選択用.ListItem.Add(
                        new C選択アイテム( name, task[(int)EタスクプロフィールＸタスク対応表見出し.タスク小分類コード]));
                });
                listBoxタスク一覧選択.ItemsSource = vmタスク一覧選択用.ListItem;
            }
        }

        private void タスク一覧選択_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            C選択アイテム item = ((sender as System.Windows.Controls.ListBox).SelectedItem as C選択アイテム);
            if( item != null)
            {
                textBoxTaskCode.Text = item.Code;
                スキル検索();

                textBox評価項目.Text = "";
                textBox評価項目.Text += "-----------------------------------\n";
                textBox評価項目.Text += "評価項目\n";
                textBox評価項目.Text += "-----------------------------------\n";
                var list = 評価項目検索(item.Code);
                if(list.Count() > 0)
                {
                    list.ForEach(tx =>{ textBox評価項目.Text += tx; });
                }

            }
            else
            {
                textBox評価項目.Text = "";
                textBoxTaskCode.Text = "N/A";
            }

        }
    }
}
//名前の由来 10/23に作成したので
//Grath Heath(グラーテ・ヘーゼ）
//https://en.wikipedia.org/wiki/Battle_of_Grathe_Heath
//