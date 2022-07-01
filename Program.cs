// See https://aka.ms/new-console-template for more information


using ResouceTest;

Console.WriteLine("请提供一个开始猜资源的地址");
Console.WriteLine("比如 http://www.baidu.com/{1}{2}/{3}.mp4");

Console.WriteLine("目前支持 {0} 年,{1} 月，{2} 日，{3} day_index");
var url = string.Empty;
var repeat = 0;
while (string.IsNullOrWhiteSpace(url))
{
    url = Console.ReadLine();
}

Console.WriteLine("请提供一个下载路径,接受默认值 C:\\Download 请输入Y");
var path = string.Empty;
while (string.IsNullOrWhiteSpace(path))
{
    var inputPath = Console.ReadLine();
    if (inputPath.Equals("y", StringComparison.OrdinalIgnoreCase))
    {
        path = "C:\\Download";
    }
    else
    {
        path = inputPath;
    }
}

Console.WriteLine("请提供一个截止日期,接受默认值 " + DateTime.Now.ToString("yyyy-MM-dd") + " 请输入Y");
DateTime? toDate = null;
while (null == toDate)
{
    var endDateStr = Console.ReadLine();
    if (endDateStr.Equals("y", StringComparison.OrdinalIgnoreCase))
    {
        toDate = DateTime.Now;
    }
    else
    {
        var c = DateTime.TryParse(endDateStr, out DateTime toDate2);
        if (c)
        {
            toDate = toDate2;
        }
    }
}


Console.WriteLine("请提供一个单日循环数量,接受默认值 100 请输入Y");
while (repeat == 0)
{
    var repeatTxt = Console.ReadLine();
    if (repeatTxt.Equals("y", StringComparison.OrdinalIgnoreCase))
    {
        repeat = 100;
    }
    else
    {
        int.TryParse(repeatTxt, out repeat);
    }
}

if (url == null)
{
    return;
}

var urls = new List<DownloadItem>();
if (url.IndexOf("{") <= 0)
{
    urls.Add(new DownloadItem()
    {
        FileName = path + "\\" + Path.GetFileName(url),
        FolderPath = path,// Path.GetTempPath(),
        Url = url
    });
}
else
{
    if (toDate == null)
    {
        toDate = DateTime.Now;
    }

    for (var i = 0; i < 100; i++)
    {
        var date = toDate.Value.AddDays(-i);
        for (var index = 1; index < 100; index++)
        {
            var newUrl = string.Format(url,
                date.ToString("yyyy"),
                date.ToString("MM"),
                date.ToString("dd"),
                index
                );
            var filename = string.Format("{0}{1}{2}-{3}.{4}",
                date.ToString("yyyy"),
                date.ToString("MM"),
                date.ToString("dd"),
                index,
                Path.GetExtension(newUrl)
                );

            var downloadItem = new DownloadItem()
            {
                FileName = path + "\\" + filename,
                FolderPath = path,// Path.GetTempPath(),
                Url = newUrl
            };

            urls.Add(downloadItem);
        }
    }
}

await Jobs.Main(urls);


while (true)
{
    Console.WriteLine("全部任务完成，请手动关闭或推出程序");
    url = Console.ReadLine();
}




