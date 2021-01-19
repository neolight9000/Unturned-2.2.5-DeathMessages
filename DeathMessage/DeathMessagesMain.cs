using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DeathMessage
{
	public class DeathMessagesMain : MonoBehaviour
	{
		private FieldInfo[] lifefields = typeof(Life).GetFields();

		private List<Player> announcedDeadPeople = new List<Player>();

		private Player[] players = Object.FindObjectsOfType<Player>();

		private float lastupdate = 0f;

		public void Start()
		{
		}

		private void CheckDeadPlayers()
		{
			try
			{
				Player[] array = players;
				foreach (Player player in array)
				{
					if ((Object)(object)player == (Object)null || (Object)(object)((Component)player).GetComponent<Life>() == (Object)null)
					{
						continue;
					}
					Life component = ((Component)player).GetComponent<Life>();
					if ((bool)lifefields[2].GetValue(component))
					{
						if (!announcedDeadPeople.Contains(player))
						{
							announcedDeadPeople.Add(player);
							string text = (string)lifefields[3].GetValue(component);
							if (text.StartsWith("You were "))
							{
								text = " was " + text.Substring(9);
							}
							else if (text.StartsWith("You "))
							{
								text = text.Substring(3);
							}
							if (text.Contains("yourself"))
							{
								text = text.Substring(0, text.IndexOf("yourself")) + "himself" + text.Substring(text.IndexOf("yourself") + 8);
							}
							if (text.Contains("your"))
							{
								text = text.Substring(0, text.IndexOf("your")) + "his" + text.Substring(text.IndexOf("your") + 4);
							}
							SendDeathMsg(player.name, text);
						}
					}
					else if (announcedDeadPeople.Contains(player))
					{
						announcedDeadPeople.Remove(player);
					}
				}
			}
			catch
			{
			}
		}

		public void Update()
		{
			if (Time.realtimeSinceStartup - lastupdate >= 3f)
			{
				players = Object.FindObjectsOfType<Player>();
				lastupdate = Time.realtimeSinceStartup;
			}
			CheckDeadPlayers();
		}

		public void OnGui()
		{
		}
		private void SendDeathMsg(string player, string deathmsg)
        {
			NetworkChat.networkChat_0.networkView.RPC("tellChat", RPCMode.All, new object[] {"Death Message", string.Empty, string.Empty, string.Concat(player,deathmsg), 2147483647, -100, -100 });
        }
	}
}
