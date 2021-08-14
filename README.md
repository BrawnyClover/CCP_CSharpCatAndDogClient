# CCP_CSharpCatAndDogClient

### Developer
YuHyun7

### Opportunity
Make a windows client which can communicate with ccp_catAndDog server

### Libraries
None(Wpf only)

### Code spec
```C#
// make HttpWebRequest instance
var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8000/blog/catdogapi/");
request.Method = "POST";
request.ContentType = "application/json";

// encode image to base64 string
string base64string = ImageToBase64(targetImage);

using (var streamWriter = new StreamWriter(request.GetRequestStream()))
{
    string json = "{"+ "\"check_image\" : \"" + base64string + "\"}";
    JsonViewer.Text = json;
    streamWriter.Write(json);
    streamWriter.Flush();
    streamWriter.Close();
    try
    {
        var response = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            StatusTextBlock.Text = result;
        }
    }
    catch
    {
        StatusTextBlock.Text = "err";
    }
}
```
