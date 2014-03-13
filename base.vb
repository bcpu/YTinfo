Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Private Declare Function GetTickCount Lib "kernel32" () As Long
    Public Overridable Property Timeout As Integer
    Public Shared Property DefaultConnectionLimit As Integer
    Public token As String
    Public title As String
    Public description As String
    Dim logincookie As CookieContainer
    Dim R As IO.StreamReader

    Private Sub GrabInfo(ByVal vid)
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create("http://www.youtube.com/watch?v=" & vid), HttpWebRequest)
        Dim tempCookies As New CookieContainer
        Dim encoding As New UTF8Encoding
        request.CookieContainer = logincookie
        request.UserAgent = "ytinfo by bcpu"
        On Error Resume Next
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        On Error Resume Next
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim theusercp As String = reader.ReadToEnd
        Dim postresponse As HttpWebResponse
        postresponse = DirectCast(request.GetResponse(), HttpWebResponse)
        tempCookies.Add(postresponse.Cookies)
        logincookie = tempCookies
        RichTextBox1.Text = theusercp

        'grab description
        Dim sourcestring As String = RichTextBox1.Text
        Dim re As Regex = New Regex("eow-description"" >(.*?)</p>")
        Dim mc As MatchCollection = re.Matches(sourcestring)
        Dim mIdx As Integer = 0
        For Each m As Match In mc
            For groupIdx As Integer = 0 To m.Groups.Count - 1
                Console.WriteLine("[{0}][{1}] = {2}", mIdx, re.GetGroupNames(groupIdx), m.Groups(groupIdx).Value)
            Next
            mIdx = mIdx + 1
        Next

        'grab title
        Dim sourcestring2 As String = RichTextBox1.Text
        Dim re2 As Regex = New Regex("dir=""ltr"" title=""(.*?)""")
        Dim mc2 As MatchCollection = re2.Matches(sourcestring2)
        Dim mIdx2 As Integer = 0
        For Each m As Match In mc2
            For groupIdx As Integer = 0 To m.Groups.Count - 1
                Console.WriteLine("[{0}][{1}] = {2}", mIdx2, re2.GetGroupNames(groupIdx), m.Groups(groupIdx).Value)
            Next
            mIdx2 = mIdx2 + 1
        Next

        description = mc(0).Groups(1).Value
        title = mc2(0).Groups(1).Value

        Console.WriteLine("Title: " & title)
        Console.WriteLine("Description: " & description)
        titlebox.Text = title
        descbox.Text = description
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        GrabInfo(vidid.Text)
    End Sub

End Class
