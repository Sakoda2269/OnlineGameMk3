import json
import random
from collections import defaultdict
from channels.generic.websocket import AsyncWebsocketConsumer
import uuid


class PlayerManager:
    players =set() # [id1, id2 ...]
    player_pos = {} #{id1 : [posx, posy, posz, rotx, roty, rotz], ...}

    async def add_player(self, ws):
        new_id = ws.channel_name[19:]
        await ws.all_send("other_join", new_id, new_player=new_id)
        await ws.personal_send("my_join", new_id, players=list(self.players))
        self.players.add(new_id)
        self.player_pos[new_id] = [0, 0, 0, 0, 0, 0]
        print(self.players)
    
    async def remove_player(self, ws):
        remove_id = ws.channel_name[19:]
        await ws.all_send("other_leave", remove_id, leave_player=remove_id)
        self.players.discard(remove_id)
        self.player_pos.pop(remove_id)
    
    async def update_position(self, ws, data):
        id = data["id"]
        pos_x = data["data"]["pos_x"]
        pos_y = data["data"]["pos_y"]
        pos_z = data["data"]["pos_z"]
        rot_x = data["data"]["rot_x"]
        rot_y = data["data"]["rot_y"]
        rot_z = data["data"]["rot_z"]
        head_x = data["data"]["head_x"]
        self.player_pos[id][0] = pos_x
        self.player_pos[id][1] = pos_y
        self.player_pos[id][2] = pos_z
        self.player_pos[id][3] = rot_x
        self.player_pos[id][4] = rot_y
        self.player_pos[id][5] = rot_z
        print(head_x)
        await ws.all_send("update", id, pos={"x":pos_x, "y":pos_y, "z":pos_z}, 
                          rot={"x":rot_x, "y":rot_y, "z":rot_z}, head={"x":head_x})
    
    async def update_animation(self, ws, data):
        id = data["id"]
        await ws.all_send("anim_update", id, param={"Speed" : data["data"]["Speed"], 
                                                    "Jump" : data["data"]["Jump"],
                                                    "Grounded": data["data"]["Grounded"],
                                                    "FreeFall" : data["data"]["FreeFall"],
                                                    "MotionSpeed" : data["data"]["MotionSpeed"]})


class EventManager:
    async def sendEvent(self, ws, data):
        id = data["id"]
        pos_x = data["data"]["pos_x"]
        pos_y = data["data"]["pos_y"]
        pos_z = data["data"]["pos_z"]
        rot_x = data["data"]["rot_x"]
        rot_y = data["data"]["rot_y"]
        rot_z = data["data"]["rot_z"]
        await ws.all_send("event", id, name=data["data"]["name"], pos={"x":pos_x, "y":pos_y, "z":pos_z}, 
                          rot={"x":rot_x, "y":rot_y, "z":rot_z})


class GameConsumer(AsyncWebsocketConsumer):
    player_manager = PlayerManager()
    event_manager = EventManager()
    async def connect(self):
        print("connect")
        await self.accept()
        self.room_id = self.scope["url_route"]["kwargs"]["room_id"]
        await self.player_manager.add_player(self)
        await self.channel_layer.group_add(
            self.room_id,
            self.channel_name
        )
        print(str(self.channel_name[19:]))
    
    async def disconnect(self, code):
        print(f"{self.channel_name[19:]} disconnect")
        await self.player_manager.remove_player(self)
        await self.channel_layer.group_discard(
            self.room_id,
            self.channel_name
        )
        return await super().disconnect(code)
    
    async def receive(self, text_data=None, bytes_data=None):
        # print(text_data)
        data = json.loads(text_data)
        method = data["method"]
        if method == "update":
            await self.player_manager.update_position(self, data)
        if method == "anim_update":
            await self.player_manager.update_animation(self, data)
        if method == "event":
            await self.event_manager.sendEvent(self, data)



    async def all_send(self, method, id, **kwargs):
        await self.channel_layer.group_send(
            self.room_id,
            {
                "type": "sender",
                "method": method,
                "id": id,
                "data":{**kwargs}
            }
        )

    async def personal_send(self, method, id, **kwargs):
        await self.send(text_data=json.dumps(
            {
                "method":method,
                "id":id,
                "data":{**kwargs}
            }
        ))

    async def sender(self, event):
        method = event["method"]
        data = event["data"]
        send_id = event["id"]
        # Send message to WebSocket
        await self.send(text_data=json.dumps({
                "method": method, 
                "data":data, 
                "id":send_id
            }))