# ʹ�� Windows Service ���b Garnet �ֲ�ʽ����

��ȿ��ʹ��΢�� Garnet ��� Redis �ֲ�ʽ���棬��ʾ����ʾ���ʹ�� Windows Service �ڵ����ϰ�װ Garnet ��������Ӧ�ó����ʹ�����������������

## ��������

```shell
sc.exe create GarnetService binpath="C:\GarnetWindowsService.exe" start= auto
```

## ��������

```shell
sc.exe start GarnetService
```

## ֹͣ����

```shell
sc.exe stop GarnetService
```

## ж�ط���

```shell
sc.exe delete GarnetService
```