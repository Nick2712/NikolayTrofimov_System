using System;


namespace System_Programming.Lesson3
{
    public class UIController : IDisposable
    {
        private UIView _view;
        private Server _server;
        private Client _client;

        public UIController(UIView view, Server server, Client client)
        {
            _view = view;
            _server = server;
            _client = client;

            _view.StartButton.onClick.AddListener(StartServer);
            _view.ShutDownButton.onClick.AddListener(ShutDownServer);
            _view.ConnectButton.onClick.AddListener(Connect);
            _view.DisconnectButton.onClick.AddListener(Disconnect);
            _view.SendButton.onClick.AddListener(SendMessage);

            _client.OnMessageReceive += ReceiveMessage;
        }

        private void StartServer()
        {
            _server.StartServer();
        }

        private void ShutDownServer()
        {
            _server.ShutDownServer();
        }

        private void Connect()
        {
            _client.Connect();
        }

        private void Disconnect()
        {
            _client.Disconnect();
        }

        private void SendMessage()
        {
            _client.SendMessage(_view.InputText.text);
            _view.InputText.text = "";
        }

        public void ReceiveMessage(object message)
        {
            _view.OutputText.text = _view.OutputText.text + "\n" + message.ToString();
        }

        public void Dispose()
        {
            _view.StartButton.onClick.RemoveAllListeners();
            _view.ShutDownButton.onClick.RemoveAllListeners();
            _view.ConnectButton.onClick.RemoveAllListeners();
            _view.DisconnectButton.onClick.RemoveAllListeners();
            _view.SendButton.onClick.RemoveAllListeners();

            _client.OnMessageReceive -= ReceiveMessage;
        }
    }
}