using Mysql.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Bussiness
{
    public class UserManager : Singleton<UserManager>
    {
        //注册
        public RegisterResult Register(RegisterType registerType, string username, string pwd)
        {
            try
            {
                if (username == " " || pwd == " ") return RegisterResult.Failed;
                //查询该用户是否存在
                int count = MySQL.Instance.sqlSugarDB.Queryable<User>().Where(x => x.Username == username).Count();
                if (count > 0)
                {
                    return RegisterResult.AlreayExit;
                }
                //没有，创建一个
                User user = new User();
                switch (registerType)
                {
                    case RegisterType.Phone:
                        user.Logintype = LoginType.Phone;
                        break;
                    case RegisterType.Mail:
                        user.Logintype = LoginType.Mail;
                        break;
                }
                //user.Id = 1;
                user.Username = username;
                user.Password = pwd;
                user.Logindate = DateTime.Now;
                //存储到mysql里面
                MySQL.Instance.sqlSugarDB.Insertable(user).ExecuteCommand();

                List<UserRole> userRoles = new List<UserRole>
        {
            new UserRole { Username = username,
            RoleName = "1111",
            MaxLv = 30,
            CurLv = 5,
            CurMP = 2*44,
            CurATK = 3*15,
            CurHP = 88*3,
            CurEXP = 200,
            CurGold = 3000,
            LastTime = DateTime.UtcNow.AddHours(8),
            SignTime = DateTime.UtcNow.AddHours(-16),
            LastIndex=-1,
            },
            new UserRole { Username = username,
            RoleName = "2222",
            MaxLv = 30,
            CurLv = 5,
            CurMP = 2 * 44,
            CurATK = 3 * 15,
            CurHP = 88 * 3,
            CurEXP = 300,
            CurGold = 4000,
            LastTime = DateTime.UtcNow.AddHours(8),
            SignTime = DateTime.UtcNow.AddHours(-16),
            LastIndex=-1,
            },
            new UserRole { Username = username,
            RoleName = "3333",
            MaxLv = 30,
            CurLv = 5,
            CurMP = 2 * 44,
            CurATK = 3 * 15,
            CurHP = 88 * 3,
            CurEXP = 350,
            CurGold = 5000,
            LastTime = DateTime.UtcNow.AddHours(8),
            SignTime = DateTime.UtcNow.AddHours(-16),
            LastIndex=-1,
            }

        };
                //新建任务
              for(int i = 0; i < 3; i++)
                {
                    List<TaskData> tasks = new List<TaskData>
                    {
                        new TaskData()
                        {
                             Uid = Guid.NewGuid().ToString(),
                             Id = 0,
                             CurNum = 0,
                             MaxNum = 3,
                             IsAccept = 1,
                             IsCancel = 0,
                             IsCompelete = 0,
                             Exp = 2000,
                             Username=username,
                             RoleName=userRoles[i].RoleName,
                        },
                        new TaskData()
                        {
                             Uid = Guid.NewGuid().ToString(),
                             Id = 1,
                             CurNum = 0,
                             MaxNum = 1,
                             IsAccept = 1,
                             IsCancel = 0,
                             IsCompelete = 0,
                             Exp = 500,
                             Username=username,
                             RoleName=userRoles[i].RoleName,
                        },
                        new TaskData()
                        {
                             Uid = Guid.NewGuid().ToString(),
                             Id = 2,
                             CurNum = 0,
                             MaxNum = 1,
                             IsAccept = 1,
                             IsCancel = 0,
                             IsCompelete = 0,
                             Exp = 5000,
                             Username=username,
                             RoleName=userRoles[i].RoleName,
                        }
                    };

                    foreach (var task in tasks)
                    {
                        int roleInsertResult = MySQL.Instance.sqlSugarDB.Insertable(task).ExecuteCommand();
                        if (roleInsertResult <= 0)
                        {
                            // 如果任何插入失败，返回失败状态
                            return RegisterResult.Failed;
                        }
                    }
                }


                foreach (var userRole in userRoles)
                {
                   int roleInsertResult = MySQL.Instance.sqlSugarDB.Insertable(userRole).ExecuteCommand();
                    if (roleInsertResult <= 0)
                    {
                        // 如果任何插入失败，返回失败状态
                        return RegisterResult.Failed;
                    }
                }

                return RegisterResult.Success;
            }
            catch (Exception e)
            {
                Debug.LogError("注册异常！" + e.ToString());
                return RegisterResult.Failed;
            }

       

            
        }

        //登录
        public LoginResult Login(LoginType loginType, string userName, string pwd, out string token)
        {
            token = "";
 
            try
            {
                User user = null;
                switch (loginType)
                {
                    case LoginType.Phone:
                    case LoginType.Mail:
                        user = MySQL.Instance.sqlSugarDB.Queryable<User>().Where(x => x.Username == userName).Single();
                        break;
                    case LoginType.WX:
                    case LoginType.QQ:

                        break;
                    case LoginType.Token:
                        break;
                }
                if (user == null)
                {
                    return LoginResult.UserNotExist;
                }
                else
                {
                    if (user.Password != pwd)
                    {
                        return LoginResult.WrongPwd;
                    }
                    else
                    {
                        user.Logindate = DateTime.UtcNow;
                        user.Token = Guid.NewGuid().ToString();
                        token = user.Token;

                        MySQL.Instance.sqlSugarDB.Updateable(user).ExecuteCommand();
                        return LoginResult.Success;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("登录异常！" + e.ToString());
                return LoginResult.Failed;
                throw;
            }

        }

        
        //查询返回用户名对应的所有角色数据
        public MsgUserRoles QueryByUsername(string usernameValue)
        {
            var db = MySQL.Instance.sqlSugarDB; // 假设这个方法返回SqlSugar的数据库实例
            var list = db.Queryable<UserRole>()
                          .Where(it => it.Username == usernameValue) // 添加条件查询
                          .ToList(); // 执行查询并返回结果列表
            MsgUserRoles roles = new MsgUserRoles();
            roles.msgUserRoles = new List<MsgUserRole>();
            roles.Username = usernameValue;
            foreach (UserRole k in list){
                //查询该角色任务、背包、技能数据
                List<CSFunData> funItems = QueryFunData(k.Username, k.RoleName);
                List<CSBagData> bagItems = QueryBagData(k.Username, k.RoleName);
                List<CSTaskData> taskItems = QueryTaskData(k.Username, k.RoleName);
                roles.msgUserRoles.Add(new MsgUserRole()
                {
                    Id = k.Id,
                    Username = k.Username,
                    RoleName = k.RoleName,
                    MaxLv = k.MaxLv,
                    CurLv = k.CurLv,
                    CurHP = k.CurHP,
                    CurStrength = k.CurStrength,
                    CurMP = k.CurMP,
                    CurATK = k.CurATK,
                    MaxGold = k.MaxGold,
                    CurGold = k.CurGold,
                    LastTime = k.LastTime,
                    SignTime = k.SignTime,
                    LastIndex=k.LastIndex,
                    FunItems=funItems,
                    BagItems=bagItems,
                    TaskItems=taskItems,
                });
            }
            return roles;
        }
        public List<CSFunData> QueryFunData(string Username,string RoleName)
        {
            var db = MySQL.Instance.sqlSugarDB;
            var list= db.Queryable<FunData>()
                      .Where(it => it.Username == Username && it.RoleName == RoleName)
                      .ToList();
            List<CSFunData> funs = new List<CSFunData>();
            foreach (FunData k in list)
            {
                funs.Add(new CSFunData()
                {
                    Id=k.Id,
                    Type=k.Type,
                    Atk = k.Atk,
                    NeedHp = k.NeedHp,
                    NeedBlue = k.NeedBlue,
                    AddHp = k.AddHp,
                    AddBlue = k.AddBlue,
                    OneLevel = k.OneLevel,
                    NeedLv = k.NeedLv,
                    IsStudy = k.IsStudy,
                    CurLv = k.CurLv,
                    NeedGold = k.NeedGold,
                    IsEquip = k.IsEquip,
                    ColdTime = k.ColdTime
                });
            }
            return funs;

        }
        public List<CSTaskData> QueryTaskData(string Username, string RoleName)
        {
            var db = MySQL.Instance.sqlSugarDB;
            var list = db.Queryable<TaskData>()
                      .Where(it => it.Username == Username && it.RoleName == RoleName)
                      .ToList();
            List<CSTaskData> tasks = new List<CSTaskData>();
            foreach (TaskData k in list)
            {
                tasks.Add(new CSTaskData()
                {
                    Uid = k.Uid,
                    Id = k.Id,
                    CurNum=k.CurNum,
                    MaxNum=k.MaxNum,
                    IsAccept=k.IsAccept,
                    IsCompelete = k.IsCompelete,
                    IsCancel=k.IsCancel,
                    Exp = k.Exp,
                    Gold=k.Gold,
                });
            }

            return tasks;
        }
        public List<CSBagData> QueryBagData(string Username, string RoleName)
        {
            var db = MySQL.Instance.sqlSugarDB;
            var list = db.Queryable<BagData>()
                      .Where(it => it.Username == Username && it.RoleName == RoleName)
                      .ToList();
            List<CSBagData> bags = new List<CSBagData>();
            foreach (BagData k in list)
            {
                bags.Add(new CSBagData()
                {
                    Uid = k.Uid,
                    Id=k.Id,
                    StarNum=k.StarNum,
                    CurLv=k.CurLv,
                    IsEquip=k.IsEquip,
                    Type=k.Type,
                    Atk=k.Atk,
                    Blue=k.Blue,
                    IsNew=k.IsNew,
                    OneLv=k.OneLv,
                });
            }

            return bags;
        }
        

        //更新角色数据
        public void UpdateUserRole(MsgUpdateUserRole msg)
        {
            try
            {
                // 根据 Username、RoleName 和 Id 查找对应的 FunData 记录
                var userRole = MySQL.Instance.sqlSugarDB.Queryable<UserRole>()
                    .Where(x => x.Username == msg.Username &&
                                x.RoleName == msg.RoleName
                                )
                    .First();

                // 如果找到了 FunData 记录
                if (userRole != null)
                {
                    userRole.CurLv = msg.CurLv;
                    userRole.CurHP = msg.CurHP;
                    userRole.CurStrength = msg.CurStrength;
                    userRole.CurMP = msg.CurMP;
                    userRole.CurATK = msg.CurATK;
                    userRole.MaxGold = msg.MaxGold;
                    userRole.CurGold = msg.CurGold;
                    userRole.LastTime = msg.LastTime;
                    userRole.SignTime = msg.SignTime;
                    userRole.LastIndex = msg.LastIndex;


                    // 使用 Updateable 方法更新记录
                    int updateResult = MySQL.Instance.sqlSugarDB.Updateable(userRole)
                        .UpdateColumns(it => new UserRole
                        {
                            MaxLv = msg.MaxLv,
                            CurLv = msg.CurLv,
                            CurHP = msg.CurHP,
                            CurStrength = msg.CurStrength,
                            CurMP = msg.CurMP,
                            CurATK = msg.CurATK,
                            MaxGold = msg.MaxGold,
                            CurGold = msg.CurGold,
                            LastTime = msg.LastTime,
                            SignTime = msg.SignTime,
                            LastIndex=msg.LastIndex,
                        })
                        .ExecuteCommand();

                    if (updateResult <= 0)
                    {
                        // 更新失败或没有找到记录，可以在这里记录日志或者抛出异常
                        return;
                    }
                }
                else
                {
                    // 如果没有找到记录，可以在这里处理，例如记录日志或返回特定消息
                    // 例如：没有找到对应的技能数据来更新
                    return;
                }

                // 更新成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("更新角色数据异常：" + e.ToString());
            }
        }

        //更新数据库操作
        //添加物品
        public void AddBagItem(MsgAddBagItem msg)
        {
            try
            {
                var bagItem = new BagData
                {
                    Username = msg.Username,
                    RoleName = msg.RoleName,
                    Uid = msg.Uid,
                    Id = msg.Id,
                    StarNum = msg.StarNum,
                    CurLv = msg.CurLv,
                    IsEquip = msg.IsEquip,
                    Type = msg.Type,
                    Atk = msg.Atk,
                    Blue=msg.Blue,
                    IsNew = msg.IsNew,
                    OneLv=msg.OneLv,
                };
                // 存储到数据库中
                int insertResult = MySQL.Instance.sqlSugarDB.Insertable(bagItem).ExecuteCommand();
                if (insertResult <= 0)
                {
                    // 如果插入失败，返回失败状态
                    return; // 或者根据需要处理失败情况
                }

                // 插入成功，返回成功状态或者进行其他业务逻辑处理
            }
            catch (Exception e)
            {
                Debug.LogError("添加背包物品异常：" + e.ToString());
                // 处理异常，例如记录日志、通知用户等
            }
        }
        //删除物品，删除字段
        public void DeleteBagItem(MsgDeleteBagItem msg)
        {
            try
            {
                // 检查必要字段是否为空
                if (string.IsNullOrWhiteSpace(msg.Username) ||
                    string.IsNullOrWhiteSpace(msg.RoleName) ||
                    string.IsNullOrWhiteSpace(msg.Uid))
                {
                    return; // 或者抛出异常
                }

                // 构建删除条件，使用Where方法来指定删除条件
                var deleteCondition = MySQL.Instance.sqlSugarDB.Deleteable<BagData>()
                    .Where(x => x.Username == msg.Username &&
                                x.RoleName == msg.RoleName &&
                                x.Uid == msg.Uid);

                // 执行删除操作，并获取影响的行数
                int affectedRows = deleteCondition.ExecuteCommand();

                if (affectedRows <= 0)
                {
                    // 如果删除失败或没有找到对应的记录，根据业务逻辑处理
                    // 例如：记录日志、返回特定消息等
                    return;
                }

                // 如果需要，可以在这里添加删除成功后的业务逻辑处理
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("删除背包物品异常：" + e.ToString());
            }
        }
        //更新物品
        public void UpdateBagItem(MsgUpdateBagItem msg)
        {
            try
            {
                // 检查必要字段是否为空或无效
                if (string.IsNullOrWhiteSpace(msg.Username) ||
                    string.IsNullOrWhiteSpace(msg.RoleName) ||
                    string.IsNullOrWhiteSpace(msg.Uid) ||
                    msg.Id <= 0)
                {
                    // 可以在这里记录日志或者抛出异常
                    return; // 缺少必要信息，不执行更新操作
                }

                // 根据 Username、RoleName、Uid 和 Id 查找对应的 BagData 记录
                var bagItemToUpdate = MySQL.Instance.sqlSugarDB.Queryable<BagData>()
                    .Where(x => x.Username == msg.Username &&
                                x.RoleName == msg.RoleName &&
                                x.Uid == msg.Uid &&
                                x.Id == msg.Id)
                    .First();

                // 如果找到了 BagData 记录
                if (bagItemToUpdate != null)
                {
                    // 更新字段内容
                    bagItemToUpdate.StarNum = msg.StarNum;
                    bagItemToUpdate.CurLv = msg.CurLv;
                    bagItemToUpdate.IsEquip = msg.IsEquip;
                    bagItemToUpdate.Type = msg.Type;
                    bagItemToUpdate.Atk = msg.Atk;
                    bagItemToUpdate.Blue = msg.Blue;
                    bagItemToUpdate.IsNew = msg.IsNew;
                    bagItemToUpdate.OneLv = msg.OneLv;

                    // 使用 Updateable 方法更新记录
                    int updateResult = MySQL.Instance.sqlSugarDB.Updateable(bagItemToUpdate)
                        .UpdateColumns(it => new BagData
                        {
                            StarNum = msg.StarNum,
                            CurLv = msg.CurLv,
                            IsEquip = msg.IsEquip,
                            Type = msg.Type,
                            Atk = msg.Atk,
                            Blue = msg.Blue,
                            IsNew = msg.IsNew,
                            OneLv = msg.OneLv
                        })
                        .ExecuteCommand();

                    if (updateResult <= 0)
                    {
                        // 更新失败或没有找到记录，可以在这里记录日志或者抛出异常
                        return;
                    }
                }
                else
                {
                    // 如果没有找到记录，可以在这里处理，例如记录日志或返回特定消息
                    // 例如：没有找到对应的物品数据来更新
                    return;
                }

                // 更新成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("更新背包物品异常：" + e.ToString());
            }
        }
        //新增新学习的技能字段
        public void StudyFun(MsgStudyFun k)
        {
                    // 如果不存在记录，则插入新学习的技能字段
                    var newFunData = new FunData
                    {
                        Username = k.Username,
                        RoleName = k.RoleName,
                        Id = k.Id,
                        Type = k.Type,
                        Atk = k.Atk,
                        NeedHp = k.NeedHp,
                        NeedBlue = k.NeedBlue,
                        AddHp = k.AddHp,
                        AddBlue = k.AddBlue,
                        OneLevel = k.OneLevel,
                        NeedLv = k.NeedLv,
                        IsStudy = k.IsStudy,
                        CurLv = k.CurLv,
                        NeedGold = k.NeedGold,
                        IsEquip = k.IsEquip,
                        ColdTime = k.ColdTime

                    };
                    int insertResult = MySQL.Instance.sqlSugarDB.Insertable(newFunData).ExecuteCommand();
                    if (insertResult <= 0)
                    {
                        // 插入失败，可以在这里记录日志或者抛出异常
                        return;
                    }
        }
        //更新技能数据字段
        public void UpFun(MsgUpFun msg)
        {
            try
            {
                // 根据 Username、RoleName 和 Id 查找对应的 FunData 记录
                var funDataToUpdate = MySQL.Instance.sqlSugarDB.Queryable<FunData>()
                    .Where(x => x.Username == msg.Username &&
                                x.RoleName == msg.RoleName &&
                                x.Id == msg.Id)
                    .First();

                // 如果找到了 FunData 记录
                if (funDataToUpdate != null)
                {
                    // 更新字段内容
                    funDataToUpdate.Atk = msg.Atk;
                    funDataToUpdate.NeedHp = msg.NeedHp;
                    funDataToUpdate.NeedBlue = msg.NeedBlue;
                    funDataToUpdate.AddHp = msg.AddHp;
                    funDataToUpdate.AddBlue = msg.AddBlue;
                    funDataToUpdate.OneLevel = msg.OneLevel;
                    funDataToUpdate.NeedLv = msg.NeedLv;
                    funDataToUpdate.IsStudy = msg.IsStudy;
                    funDataToUpdate.CurLv = msg.CurLv;
                    funDataToUpdate.NeedGold = msg.NeedGold;
                    funDataToUpdate.IsEquip = msg.IsEquip;
                    funDataToUpdate.ColdTime = msg.ColdTime;


                    // 使用 Updateable 方法更新记录
                    int updateResult = MySQL.Instance.sqlSugarDB.Updateable(funDataToUpdate)
                        .UpdateColumns(it => new FunData
                        {
                            Atk = msg.Atk,
                            NeedHp = msg.NeedHp,
                            NeedBlue = msg.NeedBlue,
                            AddHp = msg.AddHp,
                            AddBlue = msg.AddBlue,
                            OneLevel = msg.OneLevel,
                            NeedLv = msg.NeedLv,
                            IsStudy = msg.IsStudy,
                            CurLv = msg.CurLv,
                            NeedGold = msg.NeedGold,
                            IsEquip = msg.IsEquip,
                            ColdTime = msg.ColdTime
                        })
                        .ExecuteCommand();

                    if (updateResult <= 0)
                    {
                        // 更新失败或没有找到记录，可以在这里记录日志或者抛出异常
                        return;
                    }
                }
                else
                {
                    // 如果没有找到记录，可以在这里处理，例如记录日志或返回特定消息
                    // 例如：没有找到对应的技能数据来更新
                    return;
                }

                // 更新成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("更新技能数据异常：" + e.ToString());
            }
        }
        //新增接取的任务字段
        public void AddTask(MsgAddTask msg)
        {
            try
            {
                // 创建 TaskData 对象，包含所有需要添加的字段
                var newTaskData = new TaskData
                {
                    Uid = msg.Uid,
                    Id = msg.Id,
                    CurNum = msg.CurNum,
                    MaxNum = msg.MaxNum,
                    IsAccept = msg.IsAccept,
                    IsCompelete = msg.IsCompelete,
                    IsCancel = msg.IsCancel,
                    Exp = msg.Exp,
                    Gold = msg.Gold,
                    Username = msg.Username,
                    RoleName = msg.RoleName
                };
                // 使用 Insertable 方法将新任务数据添加到数据库
                int insertResult = MySQL.Instance.sqlSugarDB.Insertable(newTaskData).ExecuteCommand();

                if (insertResult <= 0)
                {
                    // 如果添加失败，可以在这里记录日志或者抛出异常
                    return;
                }

                // 添加成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("添加任务异常：" + e.ToString());
            }
        }
        //更新任务状态字段
        public void UpdateTask(MsgUpdateTask msg)
        {
            try
            {
               

                // 根据 Uid、Username、RoleName 和 Id 查找对应的 TaskData 记录
                var taskDataToUpdate = MySQL.Instance.sqlSugarDB.Queryable<TaskData>()
                    .Where(x =>
                                x.Username == msg.Username &&
                                x.RoleName == msg.RoleName &&
                                x.Id == msg.Id)
                    .First();

                // 如果找到了 TaskData 记录
                if (taskDataToUpdate != null)
                {
                    // 更新字段内容
                    taskDataToUpdate.CurNum = msg.CurNum;
                    taskDataToUpdate.MaxNum = msg.MaxNum;
                    taskDataToUpdate.IsAccept = msg.IsAccept;
                    taskDataToUpdate.IsCompelete = msg.IsCompelete;
                    taskDataToUpdate.IsCancel = msg.IsCancel;
                    taskDataToUpdate.Exp = msg.Exp;
                    taskDataToUpdate.Gold = msg.Gold;

                    // 使用 Updateable 方法更新记录
                    int updateResult = MySQL.Instance.sqlSugarDB.Updateable(taskDataToUpdate)
                        .UpdateColumns(it => new TaskData
                        {
                            CurNum = msg.CurNum,
                            MaxNum = msg.MaxNum,
                            IsAccept = msg.IsAccept,
                            IsCompelete = msg.IsCompelete,
                            IsCancel = msg.IsCancel,
                            Exp = msg.Exp,
                            Gold = msg.Gold
                        })
                        .ExecuteCommand();

                    if (updateResult <= 0)
                    {
                        // 更新失败或没有找到记录，可以在这里记录日志或者抛出异常
                        return;
                    }
                }
                else
                {
                    // 如果没有找到记录，可以在这里处理，例如记录日志或返回特定消息
                    // 例如：没有找到对应的任务数据来更新
                    return;
                }

                // 更新成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("更新任务数据异常：" + e.ToString());
            }
        }
        //删除任务字段
        public void DeleteTask(MsgDeleteTask msg)
        {
            try
            {
                // 检查必要字段是否为空或无效
                if (msg == null ||
                    string.IsNullOrWhiteSpace(msg.Username) ||
                    string.IsNullOrWhiteSpace(msg.RoleName)
                   )
                {
                    // 可以在这里记录日志或者抛出异常
                    return; // 缺少必要信息，不执行删除操作
                }

                // 构建删除条件，这里假设 Uid 足以定位唯一的任务记录
                // 如果需要，可以添加更多条件，例如 Username 和 RoleName
                var deleteCondition = MySQL.Instance.sqlSugarDB.Deleteable<TaskData>()
                    .Where(x => x.Username == msg.Username && x.RoleName == msg.RoleName && x.Id == msg.Id);

                // 如果需要根据 Username 和 RoleName 进一步限定条件，可以添加如下：
                // .Where(x => x.Username == msg.Username && x.RoleName == msg.RoleName)

                // 执行删除操作
                int deleteResult = deleteCondition.ExecuteCommand();

                if (deleteResult <= 0)
                {
                    // 如果删除失败（即没有行被影响），可以在这里记录日志或者抛出异常
                    // 这可能意味着没有找到对应的记录
                    return;
                }

                // 删除成功，可以在这里进行其他业务逻辑处理，如记录日志等
            }
            catch (Exception e)
            {
                // 记录异常信息，根据实际环境使用合适的日志记录方式
                Debug.LogError("删除任务异常：" + e.ToString());
            }
        }
    }
}
