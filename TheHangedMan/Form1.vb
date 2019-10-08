Imports System.IO

Public Class Form1

    'Dim possibleNamesToFind() As String = {"Christian", "Leopold", "Jerome", "Romain", "Ericcc", "Thibaud", "Frederic", "Viviane", "Sandra", "Charles"}
    Dim alphabet() As Char = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Dim cptr As Integer = 10
    Dim cptrLettersFound As Integer = 0
    Dim gameOver As Boolean = False

    Dim nameToFind As String
    Dim index As Integer
    Public MyImage As Bitmap

    Dim currentImg As String = "etape10"

    Dim sCharToFind As String = ""
    'Dim cCharToFind As Char
    Dim labelBoxClicked As Label
    Dim keyAlreadyPressed As Boolean = False



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer

        'Generate a random number to get the name to find in the table
        Randomize()
        'nameToFind = possibleNamesToFind((Int(possibleNamesToFind.Length - 1) * Rnd()))
        'MsgBox(nameToFind)

        'Open a text file to read all the possible names to find
        Dim resource As String
        resource = My.Resources.NamesToFind
        Dim lignes() As String = resource.Split(vbCr)
        nameToFind = lignes((Int(lignes.Length - 1) * Rnd()))
        nameToFind = nameToFind.Trim 'Delete the blanks in the string


        'Create dynamically as many textBoxes as character in the word to find
        'Each textbox contains 1 character
        For i = 0 To nameToFind.Length - 1
            Dim obj As New System.Windows.Forms.TextBox

            With obj
                .Name = "TB" & i 'nom de ta textbox (TB1, TB2, TB3, ...)
                .Left = (i) * 40 + 20 'position par rapport au rebord gauche de l'UserForm
                .Top = 20   'position par rapport au haut de l'UserForm
                .Width = 35 'largeur de la zone d'écriture
                .Height = 50 'hauteur de la zone d'écriture
                'tu peux rajouter ou enlever des propriétés de l'objet
                .Font = New Font("Microsoft Sans Serif", 14)
                .Text = ""
                .Enabled = False
                .Visible = True
            End With

            'ajout du controle à la form
            Panel2.Controls.Add(obj)

        Next

        'Create dynamically 26 textBoxes
        'Each textbox contains 1 character of the alphabet
        For i = 0 To 25
            Dim obj As New System.Windows.Forms.Label
            'Obj = Form1.Controls.Add("forms.textbox.1")
            With obj
                .Name = "L" & i 'nom de ta textbox (Txt1, Txt2, Txt3, ..., Txt26)
                .Left = (i - ((i \ 7) * 7)) * 40 + 20 'position par rapport au rebord gauche de l'UserForm
                .Top = (i \ 7) * 40 + 20   'position par rapport au haut de l'UserForm
                .Width = 30 'largeur de la zone d'écriture
                .Height = 30 'hauteur de la zone d'écriture
                .Margin = New Padding(50, 50, 50, 50)
                .TextAlign = ContentAlignment.MiddleCenter
                .Font = New Font("Microsoft Sans Serif", 12)
                .BackColor = Color.White
                .BorderStyle = BorderStyle.FixedSingle
                'tu peux rajouter ou enlever des propriétés de l'objet
                .Text = alphabet(i)
                .Visible = True
            End With

            'ajout du controle à la form
            Panel1.Controls.Add(obj)
            'definition de l'evenement click
            AddHandler obj.MouseClick, AddressOf obj_MouseClick
        Next

        'Initialise the text of the label3
        Label3.ForeColor = Color.Green
        Label3.Text = "YOU HAVE " & cptr & " TRIES LEFT"
        'Initialise the text of the label4
        Label4.Text = ""

        'The first image of the hanged man
        PictureBox1.Image = CType(My.Resources.ResourceManager.GetObject(currentImg), Image)
        'PictureBox1.Image = New Bitmap("C:\Users\CDA03\Documents\Visual Studio 2010\Projects\TheHangedMan\etape10.png")

        'definition de l'evenement keyPress pour les boutons clavier
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf form1_KeyDown

    End Sub

    Private Function getChar(ByVal e As System.Windows.Forms.KeyEventArgs) As Char
        Dim sCar As String = ""

        If e.KeyCode = Keys.A Then sCar = "A"
        If e.KeyCode = Keys.B Then sCar = "B"
        If e.KeyCode = Keys.C Then sCar = "C"
        If e.KeyCode = Keys.D Then sCar = "D"
        If e.KeyCode = Keys.E Then sCar = "E"
        If e.KeyCode = Keys.F Then sCar = "F"
        If e.KeyCode = Keys.G Then sCar = "G"
        If e.KeyCode = Keys.H Then sCar = "H"
        If e.KeyCode = Keys.I Then sCar = "I"
        If e.KeyCode = Keys.J Then sCar = "J"
        If e.KeyCode = Keys.K Then sCar = "K"
        If e.KeyCode = Keys.L Then sCar = "L"
        If e.KeyCode = Keys.M Then sCar = "M"
        If e.KeyCode = Keys.N Then sCar = "N"
        If e.KeyCode = Keys.O Then sCar = "O"
        If e.KeyCode = Keys.P Then sCar = "P"
        If e.KeyCode = Keys.Q Then sCar = "Q"
        If e.KeyCode = Keys.R Then sCar = "R"
        If e.KeyCode = Keys.S Then sCar = "S"
        If e.KeyCode = Keys.T Then sCar = "T"
        If e.KeyCode = Keys.U Then sCar = "U"
        If e.KeyCode = Keys.V Then sCar = "V"
        If e.KeyCode = Keys.W Then sCar = "W"
        If e.KeyCode = Keys.X Then sCar = "X"
        If e.KeyCode = Keys.Y Then sCar = "Y"
        If e.KeyCode = Keys.Z Then sCar = "Z"
        Return sCar

    End Function



    'IN THE FORM1 THE PROPERTY KEYPREVIEW MUST BE TRUE !!!!!!!!!!!
    Private Sub form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) 'Handles MyBase.KeyDown

        go(sender, e, 1)

    End Sub


    'The user clicked on a textbox that contains a letter of the alphabet 
    Private Sub obj_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.Click
        If TypeOf sender Is Label Then
            go(sender, e, 0)
        End If


    End Sub


    Private Sub go(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal source As System.Int32)

        Dim charArray() As Char
        keyAlreadyPressed = False

        If gameOver = False Then

            'Update the display
            If source = 1 Then 'only for key events
                sCharToFind = getChar(e)
                'sCharToFind = Char.ConvertFromUtf32(e.keyCode)
                'sCharToFind = Chr((KeyEventArgs)e.KeyCode)
                For Each txt1 As Label In Me.Panel1.Controls
                    If TypeOf txt1 Is Label Then 'if it is a textbox
                        If txt1.Text = sCharToFind Then 'if the property text of the textbox is equal to the character
                            If txt1.Enabled = False Then
                                keyAlreadyPressed = True
                            Else
                                txt1.Enabled = False
                            End If

                        End If
                    End If
                Next
            Else
                labelBoxClicked = DirectCast(sender, Label) 'only for click events
                labelBoxClicked.Enabled = False
                sCharToFind = labelBoxClicked.Text
            End If
            If keyAlreadyPressed = False Then
                nameToFind = nameToFind.ToUpper
                If nameToFind.Contains(sCharToFind) Then
                    charArray = nameToFind.ToCharArray()
                    For i = 0 To nameToFind.Length - 1
                        If charArray(i) = sCharToFind Then
                            cptrLettersFound = cptrLettersFound + 1 'increment the counter of the number of letters found
                            For Each txt2 As TextBox In Me.Panel2.Controls
                                If TypeOf txt2 Is TextBox Then 'if it is a textbox
                                    If txt2.Name = "TB" & i Then 'if the name is
                                        txt2.ForeColor = Color.Green
                                        txt2.Text = sCharToFind
                                        txt2.Enabled = False
                                    End If
                                End If
                            Next

                        End If
                    Next
                    If cptrLettersFound = nameToFind.Length Then
                        Label3.Text = "!!!!! YOU WON !!!!!!"
                        Label3.ForeColor = Color.Green
                        gameOver = True
                    End If
                Else
                    cptr = cptr - 1
                    Label3.Text = "YOU HAVE " & cptr & " TRIES LEFT"
                    currentImg = "etape" & cptr
                    PictureBox1.Image = CType(My.Resources.ResourceManager.GetObject(currentImg), Image)

                    If cptr = 1 Then
                        Label3.Text = "THIS IS YOUR LAST TRY !!!!!!"
                        currentImg = "etape" & cptr
                        PictureBox1.Image = CType(My.Resources.ResourceManager.GetObject(currentImg), Image)
                        Label3.ForeColor = Color.Red
                    End If

                    If cptr = 0 Then
                        Label3.Text = "YOU LOST !!!!!! "
                        Label4.Text = "THE NAME TO FIND WAS " & nameToFind
                        Label4.ForeColor = Color.Red
                        currentImg = "etape" & cptr
                        PictureBox1.Image = CType(My.Resources.ResourceManager.GetObject(currentImg), Image)
                        gameOver = True
                    End If

                End If

            End If
            
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'Code à décoder, il y a un problème avec cptrLettersFound !!! 
        Me.Hide()

        Dim f As New Form1
        f.Form1_Load(sender, e)
        f.Show()

    End Sub


End Class
