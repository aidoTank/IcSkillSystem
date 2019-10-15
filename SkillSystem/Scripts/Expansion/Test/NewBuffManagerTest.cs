﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CabinIcarus.IcSkillSystem.Expansion.Runtime.Buffs.Components;
using CabinIcarus.IcSkillSystem.Expansion.Runtime.Builtin.Buffs;
using CabinIcarus.IcSkillSystem.Runtime.Buffs;
using CabinIcarus.IcSkillSystem.Runtime.Buffs.Components;
using CabinIcarus.IcSkillSystem.Runtime.Buffs.Entitys;
using CabinIcarus.IcSkillSystem.Runtime.Buffs.Systems;
using CabinIcarus.IcSkillSystem.SkillSystem.Runtime.Utils;
using NUnit.Framework;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace IcSkillSystem.SkillSystem.Expansion.Tests
{
    public class NewBuffManagerTest
    {
        struct Buff:IBuffDataComponent,IBuffValueDataComponent
        {
            public float Value { get; set; }
        }
        
        ref struct Test
        {
            public int a;
        }
        
        
        [Test]
        public void RefStructTest()
        {
            Test t = new Test();

            t.a = 100;

            Test t1 = t;

            t.a = 20;
            
            Assert.GreaterOrEqual(t1.a,20);
        }
        
        [Test]
        public void 简单的添加Buff()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();

            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            buffManagerStruct.AddBuff(entity,new Buff(){});
            stop.Stop();
            Debug.Log($"Time:{stop.Elapsed} {stop.Elapsed.Milliseconds}");
            Assert.GreaterOrEqual(buffManagerStruct.GetBuffCount<Buff>(entity),1);
        }
        
        [Test]
        public void 添加Buff_10001()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            for (var i = 0; i < 10001; i++)
            {
                buffManagerStruct.AddBuff(entity,new Buff(){});
            }
            stop.Stop();
            Debug.Log($"Time:{stop.Elapsed}");
            Assert.GreaterOrEqual(buffManagerStruct.GetBuffCount<Buff>(entity),10001);
        }
        
        [Test]
        public void 添加Buff_10001_查找Value为0的()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            for (var i = 0; i < 10001; i++)
            {
                buffManagerStruct.AddBuff(entity,new Buff(){Value = i % 5});
            }
            stop.Stop();
            Debug.Log($"Add Time:{stop.Elapsed}");
            Assert.GreaterOrEqual(buffManagerStruct.GetBuffCount<Buff>(entity),10001);

            stop.Restart();

            var result = buffManagerStruct.GetBuffs(entity, new Buff() {Value = 0});
            
            stop.Stop();
            Debug.Log($"Find Time:{stop.Elapsed}");

            Assert.GreaterOrEqual(result.Count(),2001);
        }

        class TestSystem:AIcStructBuffSystem<IcSkSEntity>
        {
//            private readonly NewBuffManager _buffManager;
//
//
//            public TestSystem(NewBuffManager buffManager)
//            {
//                this._buffManager = buffManager;
//            }

            public override void Create(IcSkSEntity entity, int index)
            {
                Debug.Log("1");
//                var buff = _buffManager.GetCurrentBuffData<Buff>(index);
////                var buff = _buffManager.GetBuffData<Buff>(entity, index);
//                if ((int)buff.Value == 0)
//                {
//                    _buffManager.SetBuffData(entity,new Buff(){Value = 100}, index);
//                }
            }

            public TestSystem(IBuffManager<AIcStructBuffSystem<IcSkSEntity>,IcSkSEntity> buffManager) : base(buffManager)
            {
            }
        }
        
        class TestSystem1:AIcStructBuffSystem<IcSkSEntity>
        {
//            private readonly NewBuffManager _buffManager;
//
//
//            public TestSystem1(NewBuffManager buffManager)
//            {
//                this._buffManager = buffManager;
//            }

            public override void Create(IcSkSEntity entity, int index)
            {
                Debug.Log("2");
//                var buff = _buffManager.GetCurrentBuffData<Buff>(index);
////                var buff = _buffManager.GetBuffData<Buff>(entity, index);
//                if ((int)buff.Value == 0)
//                {
//                    _buffManager.SetBuffData(entity,new Buff(){Value = 100}, index);
//                }
            }

            public TestSystem1(IBuffManager<AIcStructBuffSystem<IcSkSEntity>,IcSkSEntity> buffManager) : base(buffManager)
            {
            }
        }
        
        [Test]
        public void 添加Buff_10001_Value为0的将他们修改为100()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            for (var i = 0; i < 100; i++)
                buffManagerStruct.AddBuffSystem(new TestSystem(new BuffManager_Struct()))
                    .AddBuffSystem(new TestSystem1(buffManagerStruct));
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            for (var i = 0; i < 1; i++)
            {
                buffManagerStruct.AddBuff(entity,new Buff(){Value = i % 5});
            }
            stop.Stop();
            Debug.Log($"Add Time:{stop.Elapsed}");
            //Assert.GreaterOrEqual(buffManager.GetBuffCount<Buff>(entity),10001);

            stop.Restart();

            var result = buffManagerStruct.GetBuffs(entity, new Buff() {Value = 100});
            
            stop.Stop();
            Debug.Log($"Find Time:{stop.Elapsed}");

            //Assert.GreaterOrEqual(result.Count(),2001);
        }
        
        [Test]
        public void 简单的添加删除Buff()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);

            var buff = new Buff(){};
            
            buffManagerStruct.AddBuff(entity,buff);
            buffManagerStruct.RemoveBuff(entity,buff);
            
            Assert.GreaterOrEqual(buffManagerStruct.GetBuffCount<Buff>(entity),0);
        }
        
        [Test]
        public void 简单的添加修改Buff_正常()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);

            var buff = new Buff(){};
            
            buffManagerStruct.AddBuff(entity,buff);
            buffManagerStruct.SetBuffData(entity,new Buff(){Value = 100}, 0);

            Assert.GreaterOrEqual(buffManagerStruct.GetBuffData<Buff>(entity,0).Value, 100);
        }
        
        [Test]
        public void 简单的添加修改Buff_BOX()
        {
            BuffManager_Struct buffManagerStruct = new BuffManager_Struct();
            IcSkSEntity entity = new IcSkSEntity();
            buffManagerStruct.AddEntity(entity);

            var buff = new Buff(){};

            buffManagerStruct.AddBuff(entity,buff);

            IBuffManager<AIcStructBuffSystem<IcSkSEntity>, IcSkSEntity> b = buffManagerStruct;
            
            b .SetBuffData(entity,new Buff(){Value = 100}, 0);

            Assert.GreaterOrEqual(b.GetBuffData<Buff>(entity,0).Value, 100);
        }


        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator NewBuffManagerTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}