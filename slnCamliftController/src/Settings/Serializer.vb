Imports System.Drawing
Imports System.Xml

Namespace Settings

    Public Class XmlSerializer

        Public Shared Function ToXml(ByVal filename As String, ByVal nameValue As KeyValuePair(Of String, Object)) As Boolean
            Dim doc As New XmlDocument
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", Nothing))
            Dim rootNode = ToXml(doc, nameValue)
            If rootNode Is Nothing Then Return False
            doc.AppendChild(rootNode)
            Try
                doc.Save(filename)
            Catch ex As SystemException When TypeOf ex Is XmlException OrElse _
                                        TypeOf ex Is ArgumentException OrElse _
                                        TypeOf ex Is IO.IOException OrElse _
                                        TypeOf ex Is UnauthorizedAccessException OrElse _
                                        TypeOf ex Is NotSupportedException OrElse _
                                        TypeOf ex Is Security.SecurityException
                Return False
            End Try
            Return True
        End Function

        Public Shared Function ToXml(ByVal doc As XmlDocument, ByVal nameValue As KeyValuePair(Of String, Object)) As XmlElement
            Dim node As XmlElement
            Dim value = nameValue.Value
            If TypeOf value Is String Then
                node = initElement(doc, "string", nameValue.Key)
                Dim valueAttribute = doc.CreateAttribute("value")
                valueAttribute.Value = value
                node.Attributes.Append(valueAttribute)
            ElseIf TypeOf value Is Integer Then
                node = initElement(doc, "int", nameValue.Key)
                Dim valueAttribute = doc.CreateAttribute("value")
                valueAttribute.Value = value
                node.Attributes.Append(valueAttribute)
            ElseIf TypeOf value Is IList(Of KeyValuePair(Of String, Object)) Then
                node = initElement(doc, "list", nameValue.Key)
                Dim lst As IList(Of KeyValuePair(Of String, Object)) = value
                For Each kvp In lst
                    Dim subNode = ToXml(doc, kvp)
                    If subNode IsNot Nothing Then node.AppendChild(subNode)
                Next
            ElseIf TypeOf value Is IDictionary(Of String, Object) Then
                node = initElement(doc, "dict", nameValue.Key)
                Dim lst As IDictionary(Of String, Object) = value
                For Each kvp In lst
                    Dim subNode = ToXml(doc, kvp)
                    If subNode IsNot Nothing Then node.AppendChild(subNode)
                Next
            Else
                Return Nothing
            End If
            Return node
        End Function
        Private Shared Function initElement(ByVal doc As XmlDocument, ByVal elementName As String, ByVal nameAttributeValue As String) As XmlElement
            Dim node = doc.CreateElement(elementName)
            If nameAttributeValue <> "" Then
                Dim nameAttribute = doc.CreateAttribute("name")
                nameAttribute.Value = nameAttributeValue
                node.Attributes.Append(nameAttribute)
            End If
            Return node
        End Function

        Public Shared Function FromXml(ByVal fileName As String) As KeyValuePair(Of String, Object)?
            Dim doc = New XmlDocument
            Try
                doc.Load(fileName)
            Catch ex As SystemException When TypeOf ex Is XmlException OrElse _
                                        TypeOf ex Is ArgumentException OrElse _
                                        TypeOf ex Is IO.IOException OrElse _
                                        TypeOf ex Is UnauthorizedAccessException OrElse _
                                        TypeOf ex Is NotSupportedException OrElse _
                                        TypeOf ex Is Security.SecurityException
                Return Nothing
            End Try
            Dim rootElement = doc.DocumentElement
            If rootElement Is Nothing Then Return Nothing
            Return FromXml(rootElement)
        End Function
        ''' <param name="node">must not be null</param>
        Private Shared Function FromXml(ByVal node As XmlNode) As KeyValuePair(Of String, Object)?
            Dim nameAttribute = node.Attributes.GetNamedItem("name")
            Dim name As String = If(nameAttribute IsNot Nothing, nameAttribute.Value, "")
            Dim value As Object
            Select Case node.Name
                Case "int"
                    Dim nullableIntValue = decodeIntValue(node)
                    If nullableIntValue Is Nothing Then Return Nothing
                    value = nullableIntValue.Value
                Case "string"
                    Dim strValue = decodeStringValue(node)
                    If strValue Is Nothing Then Return Nothing
                    value = strValue
                Case "list"
                    Dim lst = New List(Of KeyValuePair(Of String, Object))
                    For Each subNode As XmlNode In node
                        Dim nameVal = FromXml(subNode)
                        If nameVal IsNot Nothing Then lst.Add(nameVal.Value)
                    Next
                    value = lst
                Case "dict"
                    Dim dict = New Dictionary(Of String, Object)
                    For Each subNode As XmlNode In node
                        Dim nameVal = FromXml(subNode)
                        If nameVal IsNot Nothing Then dict.Add(nameVal.Value.Key, nameVal.Value.Value)
                    Next
                    value = dict
                Case Else
                    Return Nothing
            End Select
            Return New KeyValuePair(Of String, Object)(name, value)
        End Function

        Private Shared Function decodeIntValue(ByVal node As XmlNode) As Integer?
            Dim strValue = decodeStringValue(node)
            Return If(IsNumeric(strValue), Integer.Parse(strValue), Nothing)
        End Function
        Private Shared Function decodeStringValue(ByVal node As XmlNode) As String
            Dim valueAttribute = node.Attributes.GetNamedItem("value")
            Return If(valueAttribute IsNot Nothing, valueAttribute.Value, Nothing)
        End Function


        Public Shared Function NameValueToString(ByVal nameValue As KeyValuePair(Of String, Integer)) As String
            Dim name = nameValue.Key
            name = name.Replace("=", "\=") 'encode = marks
            name = name.Replace("\", "\\") 'encode \ marks
            name = name.Replace("\\=", "\=") 'undo extra encoding from previous encoding
            Dim value As String = nameValue.Value
            Return name & "=" & value
        End Function
        Public Shared Function NameValueFromString(ByVal s As String) As KeyValuePair(Of String, Integer)
            Dim kvp As New KeyValuePair(Of String, Integer)(Nothing, -1) 'invalid
            Dim nameValueArr = s.Split("=")
            If nameValueArr.Length <> 2 Then Return kvp
            Dim name = nameValueArr(0)
            name = name.Replace("\\", "\") 'decode \ marks
            name = name.Replace("\=", "=") 'decode = marks
            If Not IsNumeric(nameValueArr(1)) Then Return kvp
            Dim value As Integer = nameValueArr(1)
            Return New KeyValuePair(Of String, Integer)(name, value)
        End Function
    End Class

End Namespace
